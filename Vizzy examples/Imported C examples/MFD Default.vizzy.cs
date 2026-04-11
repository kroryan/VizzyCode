using System;
using System.Linq;
using System.Collections.Generic;
using REWJUNO;
using REWVIZZY;

// ── Program: Default ──────────────────────────────────
Vz.Init("Default");

// ── Variables ────────────────────────────────────────
// var _temp = 0;
// var windows = [];   // list;
// var _temp2 = 0;
// var startCoord = 0;
// var inputs = [];   // list;
// var coloreableWidget = [];   // list;
// var coloreableGauge = [];   // list;
// var _page = 0;
// var _colorLch = 0;
// var _sound = 0;
// var _sRatio = 0;
// var _sMax = 0;
// var _sMin = 0;
// var radarRangePrev = 0;
// var radarRange = 0;
// var _temp3 = 0;
// var zoom = 0;
// var buttons = [];   // list;
// var _temp4 = 0;
// var mapPosition = 0;
// var targetUpdate = 0;
// var isFollowing = 0;
// var _sSize = 0;
// var colorUpdate = 0;
// var time = 0;

using (new OnStart())
{
    _sound = 1;
    _colorLch = Vz.ExactEval("vec(1, 0, 0)");
    _sSize = Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Size"" style=""prop-mfd-widget""><Constant text=""_Screen"" /></CraftProperty>");
    _sRatio = Vz.ExactEval("x(v:_sSize) / y(v:_sSize)");
    _sMax = Vz.ExactEval("max(x(v:_sSize), y(v:_sSize))");
    _sMin = Vz.ExactEval("min(x(v:_sSize), y(v:_sSize))");
    _page = 0;
    targetUpdate = false;
    isFollowing = true;
    windows = Vz.CreateListRaw("INDEX,FUEL,ORBIT,RADAR,GAUGES,CONTROLS,CALCULATOR,NAVIGATION,SETTINGS");
    inputs = Vz.CreateListRaw("Slider 1,Slider 2,Throttle,Brake,Pitch,Yaw,Roll,Forward,Right,Up");
    VzRectangle background = new VzRectangle("background");
    _temp2 = Vz.ExactEval("_sMin * vec(0.05, 0.05, 0)");
    _temp = Vz.ExactEval("0.5 * v:_sSize - v:_temp2");
    VzRectangle toggle = new VzRectangle("toggle");
    Set_Widget("toggle", "", Vz.ExactEval("scale(v:_temp, vec(-1, -1, 0))"), _temp2);
    VzRectangle buttonMenu = new VzRectangle("buttonMenu");
    Set_Widget("buttonMenu", "background", Vz.ExactEval("scale(v:_temp, vec(1, -1, 0))"), _temp2);
    Vz.Broadcast(BroadCastType.Local, "window", 1);
}

// ── Custom Instructions ──────────────────────────────
var Make_Page = Vz.DeclareCustomInstruction("Make Page", "name", "modes").SetInstructions((name, modes) =>
{
    // [TODO] Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJuYW1lIiAvPg==").Size = (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuOCIgLz4=") * Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iX3NTaXplIiAvPg=="))
    // [TODO] Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJuYW1lIiAvPg==").Opacity = Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAiIC8+")
    // [TODO] Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJuYW1lIiAvPg==").Parent = "background"
    // [TODO] Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJuYW1lIiAvPg==").Subscribe(WidgetEventType.PointerClick, "mode", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJtb2RlcyIgLz4="), (d) => { })
});

using (new OnReceiveMessage("beep"))
{
    Vz.Beep(Vz.RawXmlCraftProperty(@"<CraftProperty property=""Sound.Frequency"" style=""note-frequency""><Constant text=""A"" /><Constant text=""4"" /></CraftProperty>"), _sound, 0.05);
    Vz.WaitSeconds(0.05);
    Vz.Beep(Vz.RawXmlCraftProperty(@"<CraftProperty property=""Sound.Frequency"" style=""note-frequency""><Constant text=""F"" /><Constant text=""5"" /></CraftProperty>"), _sound, 0.05);
}

var Make_Gauge = Vz.DeclareCustomInstruction("Make Gauge", "name", "parent", "position", "size").SetInstructions((name, parent, position, size) =>
{
    Set_Widget(name, parent, position, size);
    Vz.ListAdd(coloreableGauge, name);
});

using (new OnReceiveMessage("button"))
{
    using (new If((_page > -1)))
    {
        Vz.Broadcast(BroadCastType.Local, "beep", "");
        // [TODO] Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+")).Opacity = ((Vz.RawCraftProperty("PENyYWZ0UHJvcGVydHkgcHJvcGVydHk9Ik1mZC5PcGFjaXR5IiBzdHlsZT0icHJvcC1tZmQtd2lkZ2V0Ij48TGlzdE9wIG9wPSJnZXQiIHN0eWxlPSJsaXN0LWdldCI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPjxDb25zdGFudCB0ZXh0PSIyIiAvPjwvTGlzdE9wPjwvQ3JhZnRQcm9wZXJ0eT4=") == Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")) ? Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuOCIgLz4=") : Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"))
        Vz.Broadcast(BroadCastType.Local, Vz.ListGet(data, 1), Vz.ListGet(data, 2));
    }
}

var Make_Button = Vz.DeclareCustomInstruction("Make Button", "name", "parent", "position", "size", "text").SetInstructions((name, parent, position, size, text) =>
{
    Set_Widget(name, parent, position, size);
    Vz.ListAdd(coloreableWidget, name);
    Set_Widget(Vz.Join(name, "Fill", ""), name, position, Vz.ExactEval("0.9 * v:size"));
    // [TODO] Vz.Join(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJuYW1lIiAvPg=="), "Fill", "").Color = Vz.RawCraftProperty("PENyYWZ0UHJvcGVydHkgcHJvcGVydHk9Ik1mZC5Db2xvciIgc3R5bGU9InByb3AtbWZkLXdpZGdldCI+PENvbnN0YW50IHRleHQ9ImJhY2tncm91bmQiIC8+PC9DcmFmdFByb3BlcnR5Pg==")
    // [TODO] Vz.Join(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJuYW1lIiAvPg=="), "Fill", "").Subscribe(WidgetEventType.PointerClick, "button", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJwYXJlbnQiIC8+"), (d) => { })
    // [TODO] Vz.Join(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJuYW1lIiAvPg=="), "Label", "").AutoSize = Vz.RawConstant("PENvbnN0YW50IHN0eWxlPSJ0cnVlIiBib29sPSJ0cnVlIiAvPg==")
    // [TODO] Vz.Join(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJuYW1lIiAvPg=="), "Label", "").Text = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJ0ZXh0IiAvPg==")
    Set_Widget(Vz.Join(name, "Label", ""), name, position, Vz.ExactEval("0.9 * v:size"));
    Vz.ListAdd(coloreableWidget, Vz.Join(name, "Label", ""));
});

using (new OnReceiveMessage("slider"))
{
    using (new If((_page > -1)))
    {
        Vz.Beep(Vz.RawXmlCraftProperty(@"<CraftProperty property=""Sound.Frequency"" style=""note-frequency""><Constant text=""F"" /><Constant text=""5"" /></CraftProperty>"), _sound, 0.03);
        // [TODO] Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjIiIC8+")).FillAmount = (Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuNSIgLz4=") + (Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9InkobGlzdFZlYygmcXVvdDtkYXRhJnF1b3Q7LCAzKSkiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+") / Vz.RawCraftProperty("PENyYWZ0UHJvcGVydHkgcHJvcGVydHk9Ik1mZC5TaXplIiBzdHlsZT0icHJvcC1tZmQtd2lkZ2V0Ij48TGlzdE9wIG9wPSJnZXQiIHN0eWxlPSJsaXN0LWdldCI+PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPjxDb25zdGFudCB0ZXh0PSIyIiAvPjwvTGlzdE9wPjwvQ3JhZnRQcm9wZXJ0eT4=").y))
        Vz.Broadcast(BroadCastType.Local, Vz.ListGet(data, 1), Vz.ListGet(data, 2));
    }
}

var Make_Slider = Vz.DeclareCustomInstruction("Make Slider", "name", "parent", "position", "size").SetInstructions((name, parent, position, size) =>
{
    Set_Widget(name, parent, position, size);
    Set_Widget(Vz.Join(name, "Background", ""), name, position, size);
    // [TODO] Vz.Join(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJuYW1lIiAvPg=="), "Background", "").Opacity = Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuMSIgLz4=")
    // [TODO] Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJuYW1lIiAvPg==").Icon = "Ui/Sprites/Flight/ThrottleSlider"
    // [TODO] Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJuYW1lIiAvPg==").FillMethod = "Vertical"
    Vz.ListAdd(coloreableWidget, name);
    Vz.ListAdd(coloreableWidget, Vz.Join(name, "Background", ""));
    // [TODO] Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJuYW1lIiAvPg==").Subscribe(WidgetEventType.PointerDown, "slider", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJwYXJlbnQiIC8+"), (d) => { })
    // [TODO] Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJuYW1lIiAvPg==").Subscribe(WidgetEventType.Drag, "slider", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJwYXJlbnQiIC8+"), (d) => { })
});

var Set_Widget = Vz.DeclareCustomInstruction("Set Widget", "name", "parent", "position", "size").SetInstructions((name, parent, position, size) =>
{
    // [TODO] Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJuYW1lIiAvPg==").Position = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJwb3NpdGlvbiIgLz4=")
    // [TODO] Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJuYW1lIiAvPg==").Size = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJzaXplIiAvPg==")
    // [TODO] Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJuYW1lIiAvPg==").Parent = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJwYXJlbnQiIC8+")
});

using (new OnReceiveMessage("toggle"))
{
    Vz.Beep(Vz.RawXmlCraftProperty(@"<CraftProperty property=""Sound.Frequency"" style=""note-frequency""><Constant text=""A"" /><BinaryOp op=""-"" style=""op-sub""><Constant text=""5"" /><Comparison op=""="" style=""op-eq""><CraftProperty property=""Mfd.Opacity"" style=""prop-mfd-widget""><Constant text=""toggle"" /></CraftProperty><Constant text=""1"" /></Comparison></BinaryOp></CraftProperty>"), _sound, 0.1);
    Vz.WaitSeconds(0.1);
    Vz.Beep(Vz.RawXmlCraftProperty(@"<CraftProperty property=""Sound.Frequency"" style=""note-frequency""><Constant text=""F"" /><BinaryOp op=""+"" style=""op-add""><Constant text=""4"" /><Comparison op=""="" style=""op-eq""><CraftProperty property=""Mfd.Opacity"" style=""prop-mfd-widget""><Constant text=""toggle"" /></CraftProperty><Constant text=""1"" /></Comparison></BinaryOp></CraftProperty>"), _sound, 0.2);
}

using (new OnReceiveMessage("makeCONTROLS"))
{
    Make_Page("CONTROLS", 2);
    _temp = Vz.ExactEval("_sMin / ((_sRatio < 0.5 | _sRatio > 2) ? 3 : 9)");
    _temp3 = Vz.ExactEval("_sRatio >= 1 ? vec(0.25 * (0.9 * _sMax - _temp), 0, 0) : vec(0, 0.25 * (0.9 * _sMax - _temp), 0)");
    using (new For("i").From(0).To(1).By(1))
    {
        _temp4 = Vz.Join("slider", i, "");
        _temp2 = Vz.ExactEval("_temp * ((_sRatio < 0.5 | _sRatio > 2) ? (i > 0 ? -0.66666 : 0.66666) : (i > 0 ? 3 : 1))");
        Make_Slider(_temp4, "CONTROLS", Vz.ExactEval("_sRatio < 1 ? vec(_temp2, 0, 0) : vec(0, _temp2, 0)"), Vz.ExactEval("vec(_temp, 0.9 * _sMax, 0)"));
        // [TODO] Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iX3RlbXA0IiAvPg==").Rotation = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9Il9zUmF0aW8gJmx0OyAxID8gMCA6IC05MCIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=")
        _temp2 = Vz.ExactEval("_sRatio < 1 ? vec(_temp2, 0.45 * _sMax + ((_sRatio < 0.5 | _sRatio > 2) ? _sMin / 18: 0), 0) : vec(0, _temp2 - 0.75 * _temp + ((_sRatio < 0.5 | _sRatio > 2) ? _sMin / 18: 0), 0)");
        Make_Button(Vz.Join("sliderMenu", i, ""), _temp4, _temp2, Vz.ExactEval("vec(_sMin * (_sRatio >= 0 ? 2 : 1) / 9, _sMin / 18, 0)"), Vz.ExactEval("listStr(\"inputs\", i + 1)"));
        // [TODO] Vz.Join("sliderMenu", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg=="), "Fill", "").Subscribe(WidgetEventType.PointerClick, "sliderMenu", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg=="), (d) => { })
        using (new For("j").From(1).To(5).By(1))
        {
            Make_Button(Vz.Join("buttonAG", Vz.ExactEval("j + 5 * i"), ""), "CONTROLS", (Vz.ExactEval("(j - 3) * v:_temp3") - Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Position"" style=""prop-mfd-widget""><Variable list=""false"" local=""false"" variableName=""_temp4"" /></CraftProperty>")), Vz.ExactEval("vec(_temp, _temp, 0)"), Vz.ExactEval("j + 5 * i"));
        }
    }
    _page = 0;
    Vz.Broadcast(BroadCastType.Local, "CONTROLS", "page");
    using (new While(Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Exists"" style=""prop-mfd-widget""><Constant text=""CONTROLS"" /></CraftProperty>")))
    {
        using (new For("i").From(0).To(1).By(1))
        {
            using (new If((Vz.Time.TotalTime() > _temp2)))
            {
                Vz.Broadcast(BroadCastType.Local, "sliderMenu", Vz.CreateListRaw(i));
            }
        }
        Vz.WaitSeconds(0);
        using (new For("i").From(1).To(10).By(1))
        {
            // [TODO] Vz.Join("buttonAG", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg=="), "Fill", "").Opacity = (Vz.ActivationGroup(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg==")) ? Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjAuOCIgLz4=") : Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+"))
        }
        Vz.WaitSeconds(0);
    }
}

using (new OnReceiveMessage("makeCALCULATOR"))
{
    Make_Page("CALCULATOR", 1);
    VzLabel display = new VzLabel("display");
    Set_Widget("display", "CALCULATOR", Vz.ExactEval("vec(0, 0.4 * y(v:_sSize), 0)"), Vz.ExactEval("scale(v:_sSize, vec(0.8, 0.15, 0))"));
    Vz.ListAdd(coloreableWidget, "display");
    _temp2 = Vz.ExactEval("0.15 * v:_sSize");
    using (new For("row").From(0).To(3).By(1))
    {
        using (new For("column").From(1).To(4).By(1))
        {
            _temp = Vz.LetterOf(Vz.ExactEval("row * 4 + column"), "789/456*123-0.=+");
            _temp3 = Vz.Join("button", _temp, "");
            _temp4 = Vz.Join(_temp3, "Label", "");
            Set_Widget(_temp3, "CALCULATOR", Vz.ExactEval("scale(v:_sSize, vec(0.2 * column - 0.5, 0.2 - 0.2 * row, 0))"), _temp2);
            Set_Widget(_temp4, "CALCULATOR", Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Position"" style=""prop-mfd-widget""><Variable list=""false"" local=""false"" variableName=""_temp3"" /></CraftProperty>"), _temp2);
            Vz.ListAdd(coloreableWidget, _temp3);
            // [TODO] Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iX3RlbXA0IiAvPg==").Color = Vz.RawCraftProperty("PENyYWZ0UHJvcGVydHkgcHJvcGVydHk9Ik1mZC5Db2xvciIgc3R5bGU9InByb3AtbWZkLXdpZGdldCI+PENvbnN0YW50IHRleHQ9ImJhY2tncm91bmQiIC8+PC9DcmFmdFByb3BlcnR5Pg==")
            // [TODO] Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iX3RlbXA0IiAvPg==").AutoSize = Vz.RawConstant("PENvbnN0YW50IHN0eWxlPSJ0cnVlIiBib29sPSJ0cnVlIiAvPg==")
            // [TODO] Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iX3RlbXA0IiAvPg==").Text = (Vz.Contains("*", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iX3RlbXAiIC8+")) ? "x" : Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iX3RlbXAiIC8+"))
            // [TODO] Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iX3RlbXAzIiAvPg==").Subscribe(WidgetEventType.PointerClick, "CALCULATOR", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iX3RlbXAiIC8+"), (d) => { })
        }
    }
    _temp = 0;
    _temp2 = "=";
    _temp3 = "";
    _temp4 = false;
    _page = 0;
}

using (new OnReceiveMessage("CALCULATOR"))
{
    Vz.Broadcast(BroadCastType.Local, "beep", "");
    using (new If(Vz.Contains("=+-*/", Vz.ListGet(data, 1))))
    {
        using (new If(Vz.Contains("=", _temp2)))
        {
            _temp = (Vz.LengthOf(_temp3) ? _temp3 : _temp);
        }
        using (new Else())
        {
            _temp = Vz.RawXmlEval(@"<EvaluateExpression style=""evaluate-expression""><StringOp op=""join"" style=""join""><Constant text=""_temp"" /><Variable list=""false"" local=""false"" variableName=""_temp2"" /><BinaryOp op=""+"" style=""op-add""><Variable list=""false"" local=""false"" variableName=""_temp3"" /><Constant number=""0"" /></BinaryOp><Constant text="""" /></StringOp></EvaluateExpression>");
        }
        _temp2 = Vz.ListGet(data, 1);
        _temp3 = "";
        _temp4 = false;
    }
    using (new ElseIf(Vz.Contains(".", Vz.ListGet(data, 1))))
    {
        using (new If((!_temp4)))
        {
            _temp4 = true;
            _temp3 = Vz.Join((_temp3 + 0), ".", "");
        }
    }
    using (new Else())
    {
        _temp3 = Vz.Join(_temp3, Vz.ListGet(data, 1), "");
        using (new If((!_temp4)))
        {
            _temp3 = (_temp3 + 0);
        }
    }
    using (new If(Vz.Contains("=", _temp2)))
    {
    }
    using (new Else())
    {
    }
}

using (new OnReceiveMessage("makeNAVIGATION"))
{
    Make_Page("NAVIGATION", 2);
    VzNavball nav = new VzNavball("nav");
    VzMap planet = new VzMap("planet");
    VzRectangle mapOverlay = new VzRectangle("mapOverlay");
    VzRectangle mark = new VzRectangle("mark");
    VzRectangle lock = new VzRectangle("lock");
    VzRectangle unlock = new VzRectangle("unlock");
    VzRectangle craft = new VzRectangle("craft");
    VzRectangle pointer = new VzRectangle("pointer");
    VzRectangle target = new VzRectangle("target");
    VzRectangle center = new VzRectangle("center");
    VzLabel coords = new VzLabel("coords");
    _temp = Vz.ExactEval("vec(_sMin * 0.05, _sMin * 0.05, 0)");
    Set_Widget("nav", "NAVIGATION", "", Vz.ExactEval("vec(_sMin * 0.8, _sMin * 0.8, 0)"));
    Set_Widget("planet", "NAVIGATION", "", Vz.ExactEval("vec(_sMin * 0.8, _sMin * 0.8, 0)"));
    Set_Widget("mapOverlay", "planet", "", Vz.ExactEval("vec(_sMin * 0.5, _sMin * 0.5, 0)"));
    Set_Widget("mark", "nav", Vz.ExactEval("vec(0, _sMin * -0.012, 0)"), Vz.ExactEval("vec(_sMin * 0.113, _sMin * 0.033, 0)"));
    Set_Widget("lock", "nav", Vz.ExactEval("vec(_sMin * 0.45, _sMin * 0.45, 0)"), _temp);
    Set_Widget("unlock", "nav", Vz.ExactEval("vec(_sMin * -0.45, _sMin * 0.45, 0)"), _temp);
    Set_Widget("craft", "planet", "", _temp);
    Set_Widget("pointer", "planet", "", _temp);
    Set_Widget("target", "planet", Vz.ExactEval("vec(_sMin * 0.45, _sMin * 0.45, 0)"), _temp);
    Set_Widget("center", "planet", Vz.ExactEval("vec(_sMin * -0.45, _sMin * 0.45, 0)"), _temp);
    Make_Slider("zoom", "planet", Vz.ExactEval("vec(0, _sMin * 0.45, 0)"), Vz.ExactEval("vec(_sMin * 0.05, _sMin * 0.8, 0)"));
    Set_Widget("coords", "NAVIGATION", Vz.ExactEval("vec(0, _sMin * -0.475, 0)"), Vz.ExactEval("vec(_sMin * 0.8, _sMin * 0.04, 0)"));
    Vz.ListAdd(coloreableWidget, "mark");
    Vz.ListAdd(coloreableWidget, "lock");
    Vz.ListAdd(coloreableWidget, "unlock");
    Vz.ListAdd(coloreableWidget, "craft");
    Vz.ListAdd(coloreableWidget, "pointer");
    Vz.ListAdd(coloreableWidget, "target");
    Vz.ListAdd(coloreableWidget, "center");
    Vz.ListAdd(coloreableWidget, "coords");
    _temp = "";
    _temp2 = 0;
    _temp3 = 0;
    _temp4 = Vz.PosToLatLongAsl(Vz.Craft.Target.Position());
    _page = 0;
    using (new While(Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Exists"" style=""prop-mfd-widget""><Constant text=""NAVIGATION"" /></CraftProperty>")))
    {
        _temp3 = (1 + (4 * Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Sprite.FillAmount"" style=""prop-mfd-sprite""><Constant text=""zoom"" /></CraftProperty>")));
        using (new If((Vz.LengthOf(Vz.Planet(Vz.Craft.Orbit.Planet()).Parent()) == 0)))
        {
            _page = 0;
        }
        using (new If(_page))
        {
            using (new If(isFollowing))
            {
                mapPosition = Vz.PosToLatLongAsl(Vz.Craft.Nav.Position());
                _temp2 = Vz.ExactEval("vec(0, 0, 0)");
            }
            using (new Else())
            {
                mapPosition = Vz.ExactEval("vec(clamp(x(v:mapPosition) - y(v:_temp2) * FD.TimeDelta / _temp3, -89.999, 89.999), y(v:mapPosition) - x(v:_temp2) * FD.TimeDelta / _temp3, 0)");
            }
        }
        using (new Else())
        {
            _temp2 = Vz.ExactEval("vec(0, 0, 0)");
        }
        using (new If(targetUpdate))
        {
            Vz.TargetNode(Vz.ToPosition(_temp4));
        }
        Vz.WaitSeconds(0);
    }
}

using (new OnReceiveMessage("navDrag"))
{
    using (new If((_page && (Vz.ListGet(data, 1) > -1))))
    {
        isFollowing = false;
        time = Vz.Time.TotalTime();
        _temp2 = Vz.ExactEval("vec(0.1 * listNum(\"data\", 1) * (x(listVec(\"data\", 3)) - x(v:_temp)) / (time - z(v:_temp)), 0.1 * listNum(\"data\", 1) * (y(listVec(\"data\", 3)) - y(v:_temp)) / (time - z(v:_temp)), 0)");
        _temp = Vz.ExactEval("listVec(\"data\", 3) + vec(0, 0, time)");
    }
    using (new Else())
    {
        _temp = "";
    }
}

using (new OnReceiveMessage("makeSETTINGS"))
{
    Make_Page("SETTINGS", 1);
    using (new For("i").From(1).To(4).By(1))
    {
        Make_Slider(Vz.Join("slider", Vz.ListGet(Vz.CreateListRaw("Sound,L,C,H"), i), ""), "SETTINGS", Vz.ExactEval("vec(_sRatio < 1 ? -_sMin / 4.5 * (2.5 - i) : 0, 0.025 * _sMax + (_sRatio >= 1 ? _sMin / 4.5 * (2.5 - i) : 0), 0)"), Vz.ExactEval("vec(_sMin / 9, 0.8 * _sMax, 0)"));
        // [TODO] Vz.Join("slider", Vz.ListGet(Vz.CreateListRaw("Sound,L,C,H"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg==")), "").Rotation = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9Il9zUmF0aW8gJmx0OyAxID8gMCA6IC05MCIgLz48L0V2YWx1YXRlRXhwcmVzc2lvbj4=")
    }
    Make_Button("broadcast", "SETTINGS", Vz.ExactEval("vec(0, -0.45 * y(v:_sSize), 0)"), Vz.ExactEval("scale(v:_sSize, vec(0.4, 0.08, 0))"), "SYNC");
    _page = 0;
}

using (new OnReceiveMessage("SETTINGS"))
{
    using (new If(Vz.Contains(data, "broadcastFill")))
    {
        Vz.Broadcast(BroadCastType.Craft, "SETTINGS", Vz.CreateListRaw(Vz.Format("{0},{1},{2},{3}", Vz.ExactEval("x(v:_colorLch)"), Vz.ExactEval("y(v:_colorLch)"), Vz.ExactEval("z(v:_colorLch)"), _sound, "")));
    }
    using (new Else())
    {
        using (new If(Vz.ListLength(data)))
        {
            _colorLch = Vz.ExactEval("vec(listNum(\"data\", 1), listNum(\"data\", 2), listNum(\"data\", 3))");
            _sound = Vz.ListGet(data, 4);
        }
        using (new ElseIf(Vz.Contains(data, "slider")))
        {
            _sound = Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Sprite.FillAmount"" style=""prop-mfd-sprite""><Constant text=""sliderSound"" /></CraftProperty>");
            _colorLch = Vz.Vec(Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Sprite.FillAmount"" style=""prop-mfd-sprite""><Constant text=""sliderL"" /></CraftProperty>"), Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Sprite.FillAmount"" style=""prop-mfd-sprite""><Constant text=""sliderC"" /></CraftProperty>"), Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Sprite.FillAmount"" style=""prop-mfd-sprite""><Constant text=""sliderH"" /></CraftProperty>"));
        }
        Vz.Broadcast(BroadCastType.Local, "color", RGB_from(XYZ_from(Vz.ExactEval("scale(v:_colorLch + vec(1, 0, 0), vec(0.5, 1, 6.28318))"))));
    }
}

using (new OnReceiveMessage("color"))
{
    using (new If((!colorUpdate)))
    {
        colorUpdate = true;
        using (new For("i").From(1).To(Vz.ListLength(coloreableWidget)).By(1))
        {
            // [TODO] Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJjb2xvcmVhYmxlV2lkZ2V0IiAvPg=="), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg==")).Color = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==")
        }
        using (new For("i").From(1).To(Vz.ListLength(coloreableGauge)).By(1))
        {
            // [TODO] Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJjb2xvcmVhYmxlR2F1Z2UiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg==")).FillColor = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==")
            // [TODO] Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJjb2xvcmVhYmxlR2F1Z2UiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg==")).TextColor = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==")
            // [TODO] Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJjb2xvcmVhYmxlR2F1Z2UiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg==")).BackgroundColor = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjAuNSAqIGRhdGEiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+")
        }
        colorUpdate = false;
    }
}

using (new OnReceiveMessage("makeINDEX"))
{
    VzLabel INDEX = new VzLabel("INDEX");
    using (new For("i").From(3).To(Vz.ListLength(windows)).By(1))
    {
    }
    Set_Widget("INDEX", "background", "", Vz.ExactEval("0.8 * v:_sSize"));
    Vz.WaitSeconds(0);
    _temp = Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Label.FontSize"" style=""prop-mfd-label""><Constant text=""INDEX"" /></CraftProperty>");
    Vz.ListAdd(coloreableWidget, "INDEX");
    VzLabel date = new VzLabel("date");
    Set_Widget("date", "INDEX", Vz.ExactEval("vec(0, y(v:_sSize) * -0.475, 0)"), Vz.ExactEval("_sMin * vec(0.8, 0.04, 0)"));
    Vz.ListAdd(coloreableWidget, "date");
    _page = 0;
    using (new Repeat(Vz.Floor(Vz.Random(0, Vz.ExactEval("1 / FD.TimeDelta")))))
    {
        Vz.WaitSeconds(0);
    }
    using (new While(Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Exists"" style=""prop-mfd-widget""><Constant text=""INDEX"" /></CraftProperty>")))
    {
        Vz.WaitSeconds(1);
    }
}

using (new OnReceiveMessage("INDEX"))
{
    Vz.Broadcast(BroadCastType.Local, "beep", "");
    using (new If(Vz.ListGet(data, 1)))
    {
        Vz.Broadcast(BroadCastType.Local, "window", 1);
    }
    using (new Else())
    {
        Vz.Broadcast(BroadCastType.Local, "window", Vz.ExactEval("1 + ceil((y(listVec(\"data\", 3)) / (y(v:_sSize) * 0.8) - 0.5) * (1 - listLen(\"windows\")))"));
    }
}

using (new OnReceiveMessage("makeFUEL"))
{
    Make_Page("FUEL", 1);
    using (new If((_sRatio < 0.7)))
    {
        _temp2 = Vz.ExactEval("vec(0.9, 0.63, 0) * min(_sMin, _sMax / 2.8)");
        Make_Gauge("gaugeBattery", "FUEL", Vz.ExactEval("vec(0, _sMax * 0.375, 0)"), _temp2);
        Make_Gauge("gaugeMono", "FUEL", Vz.ExactEval("vec(0, _sMax * 0.125, 0)"), _temp2);
        Make_Gauge("gaugeStage", "FUEL", Vz.ExactEval("vec(0, _sMax * -0.125, 0)"), _temp2);
        Make_Gauge("gaugeTotal", "FUEL", Vz.ExactEval("vec(0, _sMax * -0.375, 0)"), _temp2);
    }
    using (new ElseIf((_sRatio > 2)))
    {
        _temp2 = Vz.ExactEval("vec(0.9, 0.63, 0) * min(_sMin / 0.7, 0.25 * _sMax)");
        Make_Gauge("gaugeBattery", "FUEL", Vz.ExactEval("vec(_sMax * -0.375, 0, 0)"), _temp2);
        Make_Gauge("gaugeMono", "FUEL", Vz.ExactEval("vec(_sMax * -0.125, 0, 0)"), _temp2);
        Make_Gauge("gaugeStage", "FUEL", Vz.ExactEval("vec(_sMax * 0.125, 0, 0)"), _temp2);
        Make_Gauge("gaugeTotal", "FUEL", Vz.ExactEval("vec(_sMax * 0.375, 0, 0)"), _temp2);
    }
    using (new Else())
    {
        _temp = Vz.ExactEval("0.5 * v:_sSize");
        _temp2 = Vz.ExactEval("vec(0.9, 0.63, 0) * min(x(v:_temp), 1.4285 * y(v:_temp))");
        Make_Gauge("gaugeBattery", "FUEL", Vz.ExactEval("scale(v:_temp, vec(-0.5, 0.5, 0))"), _temp2);
        Make_Gauge("gaugeMono", "FUEL", Vz.ExactEval("scale(v:_temp, vec(0.5, 0.5, 0))"), _temp2);
        Make_Gauge("gaugeStage", "FUEL", Vz.ExactEval("scale(v:_temp, vec(-0.5, -0.5, 0))"), _temp2);
        Make_Gauge("gaugeTotal", "FUEL", Vz.ExactEval("scale(v:_temp, vec(0.5, -0.5, 0))"), _temp2);
    }
    _page = 0;
    using (new While(Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Exists"" style=""prop-mfd-widget""><Constant text=""FUEL"" /></CraftProperty>")))
    {
        Vz.WaitSeconds(0);
    }
}

using (new OnReceiveMessage("makeORBIT"))
{
    Make_Page("ORBIT", 1);
    VzEllipse orbitLine = new VzEllipse("orbitLine");
    VzEllipse orbitFill = new VzEllipse("orbitFill");
    VzMap planet = new VzMap("planet");
    Vz.ListAdd(coloreableWidget, "orbitLine");
    VzRectangle orbitalParamsBack = new VzRectangle("orbitalParamsBack");
    VzLabel orbitalParams = new VzLabel("orbitalParams");
    Set_Widget("orbitalParamsBack", "ORBIT", Vz.ExactEval("vec(0, -0.4 * y(v:_sSize), 0)"), Vz.ExactEval("scale(v:_sSize, vec(0.8, 0.2, 0))"));
    Set_Widget("orbitalParams", "ORBIT", Vz.ExactEval("vec(0, -0.4 * y(v:_sSize), 0)"), Vz.ExactEval("scale(v:_sSize, vec(0.8, 0.2, 0))"));
    Vz.ListAdd(coloreableWidget, "orbitalParams");
    _page = 0;
    _temp3 = Vz.ExactEval("0.8 * _sMax");
    _temp2 = (2 * Vz.Planet(Vz.Craft.Orbit.Planet()).Radius());
    using (new While(Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Exists"" style=""prop-mfd-widget""><Constant text=""ORBIT"" /></CraftProperty>")))
    {
        _temp = Vz.ExactEval("_temp3 / (OD.ApoapsisAltitude + OD.PeriapsisAltitude +  _temp2)");
        Vz.WaitSeconds(0);
    }
}

using (new OnReceiveMessage("makeRADAR"))
{
    Make_Page("RADAR", 3);
    VzTexture map = new VzTexture("map");
    Vz.InitTexture(Vz.RawXmlVariable(@"<Variable list=""false"" local=""false"" variableName=""&quot;map&quot;, Vz.RawConstant(&quot;PENvbnN0YW50IHRleHQ9IjE3IiAvPg==&quot;), Vz.RawConstant(&quot;PENvbnN0YW50IHRleHQ9IjE3IiAvPg==&quot;)"" />"));
    Set_Widget("map", "RADAR", "", Vz.ExactEval("vec(0.8 * _sMin, 0.8 * _sMin, 0)"));
    Make_Slider("zoom", "RADAR", Vz.ExactEval("vec(0, 0.425 * _sMin, 0)"), Vz.ExactEval("vec(0.05 * _sMin, 0.8 * _sMin, 0)"));
    VzLabel details = new VzLabel("details");
    Set_Widget("details", "RADAR", Vz.ExactEval("vec(0, -0.425 * _sMin, 0)"), Vz.ExactEval("vec(0.8 * _sMin, 0.05 * _sMin, 0)"));
    VzRectangle craft = new VzRectangle("craft");
    Set_Widget("craft", "RADAR", "", Vz.ExactEval("vec(0.05 * _sMin, 0.05 * _sMin, 0)"));
    Vz.ListAdd(coloreableWidget, "craft");
    _page = 0;
    using (new While(Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Exists"" style=""prop-mfd-widget""><Constant text=""RADAR"" /></CraftProperty>")))
    {
        mapPosition = Vz.Craft.Nav.Position();
        _temp3 = Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Sprite.FillAmount"" style=""prop-mfd-sprite""><Constant text=""zoom"" /></CraftProperty>");
        zoom = Vz.ExactEval("pow(10000, 1.5 - _temp3) / 17");
        _temp3 = "";
        _temp = Vz.Craft.Nav.Direction();
        _temp = Vz.ExactEval("zoom * normalize(cross(v:mapPosition, v:_temp))");
        _temp2 = Vz.ExactEval("zoom * normalize(cross(v:mapPosition, v:_temp))");
        using (new If(Vz.Craft.Grounded()))
        {
            _temp3 = "GROUNDED";
        }
        using (new If((_page == 2)))
        {
        }
        using (new Else())
        {
            radarRangePrev = radarRange;
            radarRange = (Vz.ExactEval("vec(1, 1, 1)") * Vz.RawXmlCraftProperty(@"<CraftProperty property=""Terrain.Height"" style=""terrain-query""><Planet op=""toLatLongAsl"" style=""planet-to-lat-long-asl""><Variable list=""false"" local=""false"" variableName=""mapPosition"" /></Planet></CraftProperty>"));
        }
        using (new If((!Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Exists"" style=""prop-mfd-widget""><Constant text=""RADAR"" /></CraftProperty>"))))
        {
            Vz.Break();
        }
        Vz.WaitSeconds(0);
        using (new For("x").From(1).To(17).By(1))
        {
            Vz.Broadcast(BroadCastType.Local, "RADARCOL", x);
        }
        _temp4 = 0;
        using (new WaitUntil(Vz.ExactEval("_temp4 >= 17"))) { }
    }
}

using (new OnReceiveMessage("sliderMenu"))
{
    _temp = Vz.ListIndex(inputs, Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Label.Text"" style=""prop-mfd-label""><StringOp op=""join"" style=""join""><Constant text=""sliderMenu"" /><ListOp op=""get"" style=""list-get""><Variable list=""false"" local=""true"" variableName=""data"" /><Constant text=""1"" /></ListOp><Constant text=""Label"" /><Constant text="""" /></StringOp></CraftProperty>"));
    using (new If(Vz.ListGet(data, 3).x))
    {
        Vz.Broadcast(BroadCastType.Local, "beep", "");
        _temp = Vz.ExactEval("1 + ((_temp + (x(listVec(\"data\", 3)) < 0 ? listLen(\"inputs\") - 2 : 0)) % listLen(\"inputs\"))");
        // [TODO] Vz.Join("sliderMenu", Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")), "Label", "").Text = Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9InRydWUiIGxvY2FsPSJmYWxzZSIgdmFyaWFibGVOYW1lPSJpbnB1dHMiIC8+"), Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iX3RlbXAiIC8+"))
    }
    using (new If(Vz.ExactEval("_temp = 3 | _temp = 4")))
    {
        // [TODO] Vz.Join("slider", Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")), "").FillAmount = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9Il90ZW1wID0gMyA/IFRocm90dGxlIDogQnJha2UiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+")
    }
    using (new Else())
    {
        // [TODO] Vz.Join("slider", Vz.ListGet(Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg=="), Vz.RawConstant("PENvbnN0YW50IHRleHQ9IjEiIC8+")), "").FillAmount = Vz.RawEval("PEV2YWx1YXRlRXhwcmVzc2lvbiBzdHlsZT0iZXZhbHVhdGUtZXhwcmVzc2lvbiI+PENvbnN0YW50IHRleHQ9IjAuNSArIDAuNSAqIChfdGVtcCA9IDEgPyBTbGlkZXIxIDogKF90ZW1wID0gMiA/IFNsaWRlcjIgOiAoX3RlbXAgPSA1ID8gUGl0Y2ggOiAoX3RlbXAgPSA2ID8gWWF3IDogKF90ZW1wID0gNyA/IFJvbGwgOiAoX3RlbXAgPSA4ID8gVHJhbnNsYXRlRm9yd2FyZCA6IChfdGVtcCA9IDkgPyBUcmFuc2xhdGVSaWdodCA6IChfdGVtcCA9IDEwID8gVHJhbnNsYXRlVXAgOiAwKSkpKSkpKSkiIC8+PC9FdmFsdWF0ZUV4cHJlc3Npb24+")
    }
}

using (new OnReceiveMessage("makeGAUGES"))
{
    Make_Page("GAUGES", 2);
    using (new If((_sRatio < 1.5)))
    {
        _temp = Vz.ExactEval("vec(0, 0.25 * y(v:_sSize), 0)");
        _temp2 = Vz.ExactEval("min(x(v:_sSize), y(v:_sSize) / 1.4) * vec(0.9, 0.63, 0)");
    }
    using (new Else())
    {
        _temp = Vz.ExactEval("vec(-0.25 * _sMax, 0, 0)");
        _temp2 = Vz.ExactEval("min(0.5 *_sMax, _sMin / 0.7) * vec(0.9, 0.63, 0)");
    }
    Make_Gauge("gaugeSpeed", "GAUGES", _temp, _temp2);
    Make_Gauge("gaugeAltitude", "GAUGES", Vz.ExactEval("-v:_temp"), _temp2);
    _page = 0;
    using (new While(Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Exists"" style=""prop-mfd-widget""><Constant text=""GAUGES"" /></CraftProperty>")))
    {
        _temp = Vz.Planet(Vz.Craft.Orbit.Planet()).Radius();
        using (new If(_page))
        {
        }
        using (new Else())
        {
            _temp2 = Vz.Planet(Vz.Craft.Orbit.Planet()).Mass();
            _temp3 = Vz.Planet(Vz.Craft.Orbit.Planet()).AtmosphereDepth();
        }
        Vz.WaitSeconds(0);
    }
}

using (new OnReceiveMessage("CONTROLS"))
{
    using (new If(Vz.Contains(data, "buttonAG")))
    {
        Vz.SetActivationGroup(Vz.SubString(9, (Vz.LengthOf(data) - 4), data), Vz.RawXmlVariable(@"<Variable list=""false"" local=""false"" variableName=""(!Vz.ActivationGroup(Vz.SubString(Vz.RawConstant(&quot;PENvbnN0YW50IHRleHQ9IjkiIC8+&quot;), (Vz.LengthOf(Vz.RawVariable(&quot;PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==&quot;)) - Vz.RawConstant(&quot;PENvbnN0YW50IHRleHQ9IjQiIC8+&quot;)), Vz.RawVariable(&quot;PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJkYXRhIiAvPg==&quot;))))"" />"));
    }
    using (new ElseIf((Vz.Contains(data, "page") && Vz.ExactEval("_sRatio < 0.5 | _sRatio  > 2"))))
    {
        using (new For("i").From(0).To(1).By(1))
        {
            // [TODO] Vz.Join("slider", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg=="), "").Visible = Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iX3BhZ2UiIC8+")
        }
        using (new For("i").From(1).To(10).By(1))
        {
            // [TODO] Vz.Join("buttonAG", Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0idHJ1ZSIgdmFyaWFibGVOYW1lPSJpIiAvPg=="), "").Visible = (!Vz.RawVariable("PFZhcmlhYmxlIGxpc3Q9ImZhbHNlIiBsb2NhbD0iZmFsc2UiIHZhcmlhYmxlTmFtZT0iX3BhZ2UiIC8+"))
        }
    }
    using (new Else())
    {
        _temp2 = (Vz.Time.TotalTime() + 0.2);
        _temp = Vz.ListIndex(inputs, Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Label.Text"" style=""prop-mfd-label""><StringOp op=""join"" style=""join""><Constant text=""sliderMenu"" /><StringOp op=""letter"" style=""letter""><StringOp op=""length"" style=""length""><Variable list=""false"" local=""true"" variableName=""data"" /></StringOp><Variable list=""false"" local=""true"" variableName=""data"" /></StringOp><Constant text=""Label"" /><Constant text="""" /></StringOp></CraftProperty>"));
        using (new If((_temp == 1)))
        {
            Vz.SetInput(CraftInput.Slider1, ((2 * Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Sprite.FillAmount"" style=""prop-mfd-sprite""><Variable list=""false"" local=""true"" variableName=""data"" /></CraftProperty>")) - 1));
        }
        using (new ElseIf((_temp == 2)))
        {
            Vz.SetInput(CraftInput.Slider2, ((2 * Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Sprite.FillAmount"" style=""prop-mfd-sprite""><Variable list=""false"" local=""true"" variableName=""data"" /></CraftProperty>")) - 1));
        }
        using (new ElseIf((_temp == 3)))
        {
            Vz.SetInput(CraftInput.Throttle, Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Sprite.FillAmount"" style=""prop-mfd-sprite""><Variable list=""false"" local=""true"" variableName=""data"" /></CraftProperty>"));
        }
        using (new ElseIf((_temp == 4)))
        {
            Vz.SetInput(CraftInput.Brake, Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Sprite.FillAmount"" style=""prop-mfd-sprite""><Variable list=""false"" local=""true"" variableName=""data"" /></CraftProperty>"));
        }
        using (new ElseIf((_temp == 5)))
        {
            Vz.SetInput(CraftInput.Pitch, ((2 * Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Sprite.FillAmount"" style=""prop-mfd-sprite""><Variable list=""false"" local=""true"" variableName=""data"" /></CraftProperty>")) - 1));
        }
        using (new ElseIf((_temp == 6)))
        {
            Vz.SetInput(CraftInput.Yaw, ((2 * Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Sprite.FillAmount"" style=""prop-mfd-sprite""><Variable list=""false"" local=""true"" variableName=""data"" /></CraftProperty>")) - 1));
        }
        using (new ElseIf((_temp == 7)))
        {
            Vz.SetInput(CraftInput.Roll, ((2 * Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Sprite.FillAmount"" style=""prop-mfd-sprite""><Variable list=""false"" local=""true"" variableName=""data"" /></CraftProperty>")) - 1));
        }
        using (new ElseIf((_temp == 8)))
        {
            Vz.SetInput(CraftInput.TranslateForward, ((2 * Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Sprite.FillAmount"" style=""prop-mfd-sprite""><Variable list=""false"" local=""true"" variableName=""data"" /></CraftProperty>")) - 1));
        }
        using (new ElseIf((_temp == 9)))
        {
            Vz.SetInput(CraftInput.TranslateRight, ((2 * Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Sprite.FillAmount"" style=""prop-mfd-sprite""><Variable list=""false"" local=""true"" variableName=""data"" /></CraftProperty>")) - 1));
        }
        using (new ElseIf((_temp == 10)))
        {
            Vz.SetInput(CraftInput.TranslateUp, ((2 * Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Sprite.FillAmount"" style=""prop-mfd-sprite""><Variable list=""false"" local=""true"" variableName=""data"" /></CraftProperty>")) - 1));
        }
    }
}

using (new OnReceiveMessage("RADARCOL"))
{
    using (new For("y").From(1).To(17).By(1))
    {
        DrawRadarPix(data, y, Vz.ExactEval("v:mapPosition + (data - 9) * v:_temp + (y - 9) * v:_temp2"));
    }
    _temp4 += 1;
}

var DrawRadarPix = Vz.DeclareCustomInstruction("DrawRadarPix", "x", "y", "coord").SetInstructions((x, y, coord) =>
{
    using (new If((_page == 2)))
    {
        _temp3 = Vz.RawXmlCraftProperty(@"<CraftProperty property=""Terrain.Color"" style=""terrain-query""><Planet op=""toLatLongAsl"" style=""planet-to-lat-long-asl""><Variable list=""false"" local=""true"" variableName=""coord"" /></Planet></CraftProperty>");
    }
    using (new Else())
    {
        _temp3 = Vz.RawXmlCraftProperty(@"<CraftProperty property=""Terrain.Height"" style=""terrain-query""><Planet op=""toLatLongAsl"" style=""planet-to-lat-long-asl""><Variable list=""false"" local=""true"" variableName=""coord"" /></Planet></CraftProperty>");
        radarRange = Vz.ExactEval("vec(max(_temp3, x(v:radarRange)), min(_temp3, y(v:radarRange)), 0)");
    }
});

using (new OnChangeSoi())
{
    using (new If(Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Exists"" style=""prop-mfd-widget""><Constant text=""planet"" /></CraftProperty>")))
    {
        _temp2 = (2 * Vz.Planet(planet).Radius());
    }
}

using (new OnReceiveMessage("navButton"))
{
    Vz.Broadcast(BroadCastType.Local, "beep", "");
    using (new If(Vz.ExactEval("listNum(\"data\", 1) = 1")))
    {
        Vz.LockNavSphere(LockNavSphereIndicatorType.None);
    }
    using (new ElseIf(Vz.ExactEval("listNum(\"data\", 1) = 2")))
    {
        Vz.LockNavSphere(LockNavSphereIndicatorType.Current);
    }
    using (new ElseIf(Vz.ExactEval("listNum(\"data\", 1) = 3")))
    {
        _temp4 = Vz.UserInput("Name of an existing craft, location or planet to target");
        Vz.TargetNode(_temp4);
        Vz.WaitSeconds(0);
        using (new If(Vz.Contains(Vz.Craft.Target.Planet(), Vz.Craft.Orbit.Planet())))
        {
            mapPosition = Vz.PosToLatLongAsl(Vz.Craft.Target.Position());
            isFollowing = false;
        }
        targetUpdate = false;
        _temp2 = Vz.ExactEval("vec(0, 0, 0)");
    }
    using (new ElseIf(Vz.ExactEval("listNum(\"data\", 1) = 4")))
    {
        isFollowing = true;
    }
}

using (new OnReceiveMessage("mode"))
{
    using (new If(Vz.ExactEval("_page > -1 & listNum(\"data\", 1) > 1")))
    {
        Vz.Broadcast(BroadCastType.Local, "beep", "");
        _page = Vz.ExactEval("(_page + 1) % listNum(\"data\", 1)");
        Vz.Broadcast(BroadCastType.Local, Vz.ListGet(data, 2), "page");
    }
}

using (new OnReceiveMessage("window"))
{
    using (new If(((_page > -1) && (!Vz.RawXmlCraftProperty(@"<CraftProperty property=""Mfd.Visible"" style=""prop-mfd-widget""><ListOp op=""get"" style=""list-get""><Variable list=""true"" local=""false"" variableName=""windows"" /><Variable list=""false"" local=""true"" variableName=""data"" /></ListOp></CraftProperty>")))))
    {
        _page = -1;
        using (new If(("" < 10)))
        {
            var = 0;
        }
        Vz.ListClear(coloreableWidget);
        Vz.ListClear(coloreableGauge);
        using (new For("i").From(1).To(Vz.ListLength(windows)).By(1))
        {
            Vz.DestroyWidget(Vz.ListGet(windows, i));
        }
        coloreableWidget = Vz.CreateListRaw("toggle,buttonMenu");
        Vz.WaitSeconds(0);
        Vz.Broadcast(BroadCastType.Local, Vz.Join("make", Vz.ListGet(windows, data), ""), "");
        using (new WaitUntil((_page > -1))) { }
        Vz.Broadcast(BroadCastType.Local, "SETTINGS", "");
    }
}

// ── Custom Expressions ───────────────────────────────
var XYZ_normalise = Vz.DeclareCustomExpression("XYZ normalise", "parameter").SetReturn((parameter) =>
{
    return Vz.ExactEval("(parameter * parameter * parameter) > 0.008856 ? (parameter * parameter * parameter) : ((parameter - 16) / 903.292)");
});

var XYZ_from = Vz.DeclareCustomExpression("XYZ from", "Lch").SetReturn((Lch) =>
{
    return Vz.Vec((0.94811 * XYZ_normalise(Vz.ExactEval("(x(v:Lch) + 0.16) / 1.16 + 0.2 * (y(v:Lch) * cos(z(v:Lch)))"))), XYZ_normalise(Vz.ExactEval("(x(v:Lch) + 0.16) / 1.16")), (1.07304 * XYZ_normalise(Vz.ExactEval("(x(v:Lch) + 0.16) / 1.16 - 0.5 * (y(v:Lch) * sin(z(v:Lch)))"))));
});

var RGB_normalise = Vz.DeclareCustomExpression("RGB normalise", "parameter").SetReturn((parameter) =>
{
    return Vz.ExactEval("parameter > 0.0031308 ? (1.055 * pow(parameter, 0.41666667)) - 0.055 : (12.92 * parameter)");
});

var RGB_from = Vz.DeclareCustomExpression("RGB from", "XYZ").SetReturn((XYZ) =>
{
    return Vz.Vec(RGB_normalise(Vz.ExactEval("dot(v:XYZ, vec(3.2406, -1.5372, -0.4986))")), RGB_normalise(Vz.ExactEval("dot(v:XYZ, vec(-0.9689, 1.8758, 0.0415))")), RGB_normalise(Vz.ExactEval("dot(v:XYZ, vec(0.0557, -0.204, 1.057))")));
});


// ── Serialize output ──
Console.WriteLine(Vz.context.currentProgram.Serialize().ToString());
