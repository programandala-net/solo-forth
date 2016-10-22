# gplusdos.bas

# Solo Forth loader for G+DOS

# This file is part of Solo Forth
# http://programandala.net/en.program.solo_forth.html

# This file is written in Sinclair BASIC, in Russell Marks' zmakebas
# format.

# Note: The symbols `24063`, `24064`, `24064` and `24067`
# are converted to their actual values, extracted from the Z80 symbols
# file by a Forth program called by Makefile.

1 CLEAR VAL "24063": LOAD d*"solo.bin" CODE VAL"24064"
2 POKE@VAL"10",NOT PI:RANDOMIZE USR VAL "24064": REM cold
3 RANDOMIZE USR VAL"24067": REM warm

# vim: ft=sinclairbasic
