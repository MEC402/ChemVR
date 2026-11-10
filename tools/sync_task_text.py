#!/usr/bin/env python3
"""Regenerate the popup text in Chemical_Change_Overview.cs from docs/chemmodule_tasks.md.

Edit the numbered task lines in the doc, then run:   python3 tools/sync_task_text.py
Task line format (keep it exactly):   N. **Title** (station X): instruction text
Run from the repo root. Prints a warning if the number of tasks differs from the prefabs listed in
Chemical_Change_Task.asset (add or remove a prefab there too when you add or remove a task).
"""
import re, sys

DOC = "docs/chemmodule_tasks.md"
CS = "Assets/Resources/Tasks/Chemical_Change/Chemical_Change_Overview.cs"
ASSET = "Assets/Resources/Tasks/Chemical_Change/Chemical_Change_Task.asset"
DONE = "This concludes the Physical and Chemical Changes module. Thank you for your time.\n\n(Y) Hides Popup\n(Hamburger Button) Opens Menu"

pat = re.compile(r"^(\d+)\. \*\*(.+?)\*\* \(station (.+?)\): (.+)$")
tasks = []
for line in open(DOC, encoding="utf-8"):
    m = pat.match(line.rstrip("\n"))
    if m:
        tasks.append(m.groups())
if not tasks:
    sys.exit("No task lines found in " + DOC)
for i, (n, *_rest) in enumerate(tasks, 1):
    if int(n) != i:
        sys.exit("Task numbering broken: expected %d, found %s" % (i, n))

def esc(t):
    return t.replace("\\", "\\\\").replace('"', '\\"').replace("\n", "\\n")

N = len(tasks)

def arr(var, key):
    out = ["    string[] %s = {" % var]
    for i, (n, title, st, ins) in enumerate(tasks):
        t = "Task %d of %d: %s\\nStation: %s\\n\\n%s\\n\\n[placeholder task - press %s to continue]" % (i + 1, N, esc(title), st, esc(ins), key)
        out.append('                    /*%d*/"%s",' % (i, t))
    out.append('                    /*%d*/"%s"' % (N, esc(DONE)))
    out.append("            };")
    return "\n".join(out)

src = open(CS, encoding="utf-8").read()
a = src.index("    // VR wording (controllers)")
b = src.index("    void AdvanceCheTask")
new = "    // VR wording (controllers)\n" + arr("text", "A") + "\n\n    // WebGL wording (keyboard / mouse)\n" + arr("webGLText", "N") + "\n\n"
open(CS, "w", encoding="utf-8").write(src[:a] + new + src[b:])

prefabs = sum(1 for l in open(ASSET) if l.startswith("  - {fileID: 1100000000000000001"))
print("Wrote %d tasks (VR + WebGL) to %s" % (N, CS))
if prefabs != N:
    print("WARNING: %s lists %d task prefabs but the doc has %d tasks. Add/remove prefabs to match." % (ASSET, prefabs, N))
