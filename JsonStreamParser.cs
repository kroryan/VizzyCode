using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace VizzyCode
{
    public enum StreamEventType
    {
        Delta,
        ToolUse,
        ToolResult,
        Error,
        Done
    }

    public class StreamEvent
    {
        public StreamEventType Type { get; set; }
        public string? Content { get; set; }
        public string? ToolName { get; set; }
        public Dictionary<string, object>? ToolArguments { get; set; }
        public string? ToolId { get; set; }
        public string? Error { get; set; }
    }

    public class JsonStreamParser
    {
        private readonly StringBuilder _buffer = new();
        private int _openBraces = 0;
        private int _openBrackets = 0;
        private bool _inString = false;
        private bool _escaped = false;

        public IEnumerable<StreamEvent> Parse(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) yield break;

            JsonDocument? doc = null;
            
            try
            {
                if (line.StartsWith("data:"))
                {
                    string json = line[5..].Trim();
                    if (json == "[DONE]") yield break;

                    doc = JsonDocument.Parse(json);
                }
                else
                {
                    doc = JsonDocument.Parse(line);
                }
            }
            catch (JsonException)
            {
                yield break;
            }

            if (doc != null)
            {
                foreach (var evt in ParseJsonDocument(doc))
                {
                    yield return evt;
                }
            }
        }

        private IEnumerable<StreamEvent> ParseJsonDocument(JsonDocument doc)
        {
            var root = doc.RootElement;

            if (root.ValueKind == JsonValueKind.Object)
            {
                if (root.TryGetProperty("type", out var typeProp))
                {
                    string type = typeProp.GetString() ?? "";
                    switch (type)
                    {
                        case "stream_event":
                            if (root.TryGetProperty("event", out var eventProp))
                            {
                                foreach (var evt in ParseStreamEvent(eventProp))
                                {
                                    yield return evt;
                                }
                            }
                            break;
                        case "tool_use":
                            yield return new StreamEvent
                            {
                                Type = StreamEventType.ToolUse,
                                ToolName = root.GetProperty("name").GetString(),
                                ToolArguments = ParseArguments(root.GetProperty("input")),
                                ToolId = root.GetProperty("id").GetString()
                            };
                            break;
                        case "content_block_delta":
                            if (root.TryGetProperty("delta", out var delta))
                            {
                                if (delta.TryGetProperty("type", out var deltaType) && deltaType.GetString() == "text_delta")
                                {
                                    if (delta.TryGetProperty("text", out var text))
                                    {
                                        yield return new StreamEvent
                                        {
                                            Type = StreamEventType.Delta,
                                            Content = text.GetString()
                                        };
                                    }
                                }
                            }
                            break;
                        case "result":
                            if (root.TryGetProperty("result", out var result))
                            {
                                yield return new StreamEvent
                                {
                                    Type = StreamEventType.ToolResult,
                                    Content = result.GetString() ?? result.GetRawText()
                                };
                            }
                            break;
                        default:
                            if (root.TryGetProperty("content", out var content) && content.ValueKind == JsonValueKind.String)
                            {
                                yield return new StreamEvent
                                {
                                    Type = StreamEventType.Delta,
                                    Content = content.GetString()
                                };
                            }
                            break;
                    }
                }
                else if (root.TryGetProperty("choices", out var choices))
                {
                    foreach (var choice in choices.EnumerateArray())
                    {
                        if (choice.TryGetProperty("delta", out var delta))
                        {
                            if (delta.TryGetProperty("content", out var contentArray))
                            {
                                foreach (var contentItem in contentArray.EnumerateArray())
                                {
                                    if (contentItem.TryGetProperty("text", out var text))
                                    {
                                        yield return new StreamEvent
                                        {
                                            Type = StreamEventType.Delta,
                                            Content = text.GetString()
                                        };
                                    }
                                    else if (contentItem.TryGetProperty("tool_calls", out var toolCalls))
                                    {
                                        foreach (var toolCall in toolCalls.EnumerateArray())
                                        {
                                            yield return ParseToolCall(toolCall);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private IEnumerable<StreamEvent> ParseStreamEvent(JsonElement eventElement)
        {
            if (!eventElement.TryGetProperty("type", out var typeProp)) yield break;
            string type = typeProp.GetString() ?? "";

            switch (type)
            {
                case "content_block_delta":
                    if (eventElement.TryGetProperty("delta", out var delta))
                    {
                        if (delta.TryGetProperty("type", out var deltaType) && deltaType.GetString() == "text_delta")
                        {
                            if (delta.TryGetProperty("text", out var textProp))
                            {
                                yield return new StreamEvent
                                {
                                    Type = StreamEventType.Delta,
                                    Content = textProp.GetString()
                                };
                            }
                        }
                    }
                    break;
                case "text_delta":
                    if (eventElement.TryGetProperty("text", out var text))
                    {
                        yield return new StreamEvent
                        {
                            Type = StreamEventType.Delta,
                            Content = text.GetString()
                        };
                    }
                    break;
                case "tool_use":
                    if (eventElement.TryGetProperty("name", out var toolName))
                    {
                        yield return new StreamEvent
                        {
                            Type = StreamEventType.ToolUse,
                            ToolName = toolName.GetString(),
                            ToolArguments = eventElement.TryGetProperty("input", out var input) ? ParseArguments(input) : null,
                            ToolId = eventElement.TryGetProperty("id", out var id) ? id.GetString() : null
                        };
                    }
                    break;
            }
        }

        private StreamEvent ParseToolCall(JsonElement toolCall)
        {
            var evt = new StreamEvent
            {
                Type = StreamEventType.ToolUse,
                ToolId = toolCall.TryGetProperty("id", out var id) ? id.GetString() : null
            };

            if (toolCall.TryGetProperty("type", out var typeProp) && typeProp.GetString() == "function")
            {
                if (toolCall.TryGetProperty("function", out var func))
                {
                    evt.ToolName = func.TryGetProperty("name", out var name) ? name.GetString() : null;
                    evt.ToolArguments = func.TryGetProperty("arguments", out var args) 
                        ? ParseArguments(args) 
                        : null;
                }
            }

            return evt;
        }

        private Dictionary<string, object> ParseArguments(JsonElement element)
        {
            var dict = new Dictionary<string, object>();
            foreach (var prop in element.EnumerateObject())
            {
                dict[prop.Name] = ParseJsonValue(prop.Value);
            }
            return dict;
        }

        private object ParseJsonValue(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => element.GetString() ?? "",
                JsonValueKind.Number => element.TryGetInt32(out var i) ? i : element.GetDouble(),
                JsonValueKind.True => true,
                JsonValueKind.False => false,
                JsonValueKind.Null => null,
                JsonValueKind.Array => element.EnumerateArray().Select(ParseJsonValue).ToArray(),
                JsonValueKind.Object => element.EnumerateObject()
                    .ToDictionary(p => p.Name, p => ParseJsonValue(p.Value)),
                _ => element.ToString()
            };
        }
    }
}
