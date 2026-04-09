# AI Repair Context Guide

This document exists to give future AI agents and maintainers the exact context needed to debug failing Vizzy exports without damaging fidelity-sensitive missions.

## Core Principle

Do not treat every failing export as the same kind of problem.

There are two fundamentally different cases:

1. `XML -> code -> XML` fidelity failure
2. `code -> XML` authoring failure

If those are confused, the AI will often "fix" the wrong thing and make the mission worse.

## Required Context For AI

If you want an AI to work reliably on VizzyCode, do not provide only the current code file.

Minimum documentation context:

- `README.md`
- `docs/VizzyAuthoringGuide.md`
- `docs/VizzyBlocksMegaGuide.md`
- `docs/AiRepairContextGuide.md`
- `docs/RawPreservationGuide.md`

Recommended additional Vizzy language reference:

- `docs/Mastering Vizzy _ A Complete Guide - Early Access 08.07.25.md`

For fidelity-sensitive work, also provide:

- the original working XML
- the current `.cs`
- the currently exported XML if one exists

If the file contains preserved raw fragments, also provide whether they are:

- readable `RawXml*`
- or legacy base64 `Raw*`

## The First Question To Ask

Before changing code, determine:

- is the original XML still the source of truth
- or has the `.cs` become the source of truth

## How To Recognize A Fidelity Region

A fidelity-sensitive imported region usually still contains preserved metadata comments:

- `VZTOPBLOCK`
- `VZBLOCK ...`
- `VZEL ...`

Those comments are serialized anchors for the original Vizzy graph.

If a region still has them, do not rewrite that region into cleaner code unless you are prepared to lose strict XML preservation there.

The same rule applies to `RawXml*` and legacy `Raw*` fragments inside preserved regions.

## How To Recognize An Authoring Region

A region has effectively become authoring-mode code when:

- its original `VZEL` comments are missing
- the logic was manually rewritten into new helper functions
- new top-level `CustomInstruction` or `CustomExpression` blocks were added manually
- control flow was replaced with a different but logically similar structure

In that case, the exporter is no longer preserving an imported graph there. It is creating a new one.

That does not automatically mean every `RawXml*` inside the file should be removed. A mixed mission can contain both:

- handwritten authoring regions
- preserved raw fragments that still need exact XML fidelity

## What Happened In T.T-Class Failures

`T.T` is the best warning example in this repository.

The current `T.T.cs` can contain:

- preserved imported mission blocks
- handwritten rewritten orbital logic
- newly authored top-level custom blocks

That makes it a mixed mission:

- part round-trip fidelity work
- part authoring-mode export work

In such a file, the correct question is:

- which exact failing region is still imported
- and which exact failing region is now handwritten

## Mandatory Diagnostic Sequence

When Juno does not show the exported XML in the Vizzy editor, follow this order:

1. export the same `.cs` with the repo CLI
2. export the same `.cs` with the VS Code plugin CLI
3. compare both XML files byte for byte
4. if they differ, fix tool drift first
5. if they match, inspect the `.cs` in the failing region for preserved `VZEL` metadata
6. compare the failing XML structure against working non-exported XML examples from `Vizzy examples`
7. fix one exact structural issue at a time

If raw-preserved fragments are involved:

8. decode or inspect the raw fragment instead of guessing what it contains
9. only replace it if the replacement is proven Juno-safe

Do not skip step 5.

## Things The AI Must Not Do

Do not:

- globally refactor a mission-scale file
- replace preserved raw calls with prettier code without proving the XML remains valid
- replace `RawXml*` with cleaner high-level math just because it looks nicer
- remove `VZTOPBLOCK`, `VZBLOCK`, or `VZEL` comments from imported regions
- assume that logical equivalence is enough for Juno
- assume that because the XML "looks reasonable" Juno will accept it

## Things The AI Should Check First

For authoring-created top-level blocks, check:

- `CustomInstruction` header shape
- `CustomExpression` header shape
- presence of `pos` where Juno expects it
- exact `callFormat` spacing
- exact `format` spacing
- event and top-level block ordering

For control flow, check:

- `Else` encoded in the Juno-compatible form used by this project
- top-level block starts with `Event` or `CustomInstruction`
- no stray top-level instruction block begins with a normal instruction

For fidelity-sensitive expressions, check:

- `StringOp("friendly", ...)`
- nested `Planet(...).Property()` inside arithmetic
- `DisplayMessage` / `LogMessage` argument shapes
- text constants where whitespace matters
- whether a `RawXml*` fragment is still the correct preservation boundary

For imported readable raw fragments, also check:

- whether the fragment is still exact XML from the source
- whether an AI accidentally converted it into unsupported "clean" code

## Prompt Template For AI Repair

Use this prompt when asking an AI to repair a failing mission:

```text
Investigate this Vizzy mission export without rewriting it blindly.

Your job is to determine whether the failure is caused by:
- the current .cs source
- the exporter
- or tool drift between the repo CLI and the VS Code plugin CLI

Rules:
- First compare repo CLI export and plugin CLI export byte for byte.
- Then inspect the failing region in the .cs and determine whether it still preserves VZTOPBLOCK / VZBLOCK / VZEL metadata.
- If the region still preserves metadata, preserve that region and fix the exporter.
- If the region no longer preserves metadata, treat it as authoring-mode code and compare the exported XML against real working XML examples from the repo.
- Do not globally clean up or refactor the mission.
- Fix one exact XML structural problem at a time and re-export after each fix.

Important:
- Logical equivalence is not enough. The exported XML must use the exact Juno-compatible structure.
- In mixed missions such as T.T, do not assume the original XML and the current .cs still describe the same graph.
- If the file contains `RawXml*`, treat those as explicit fidelity boundaries unless proven otherwise.
```

## Short Prompt For Fast Use

```text
Do not rewrite this mission globally.
First determine whether the failing region still has VZEL metadata.
If yes, preserve it and fix the exporter.
If no, treat it as authoring-mode code and compare its exported XML against working XML examples.
Also compare repo CLI export and plugin CLI export byte for byte before making changes.
Fix one exact structural XML discrepancy at a time.
```

## Prompt Add-On For Raw Fragments

Use this add-on when the file contains `RawXml*` or legacy `Raw*`:

```text
This file contains preserved raw XML fragments.
Do not rewrite them blindly.
First determine what exact XML they encode, whether they came from imported XML, and whether they are still required as fidelity boundaries.
Prefer converting legacy base64 Raw* into readable RawXml* only if the exported XML remains identical.
```
