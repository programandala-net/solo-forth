# trdos.bas

# Solo Forth loader for TR-DOS

# This file is part of Solo Forth
# http://programandala.net/en.program.solo_forth.html

# This file is written in Sinclair BASIC, in Russell Marks' zmakebas
# format.

# Note: The symbols `26111`, `26112`, `26112` and `26115`
# are converted to their actual values, extracted from the Z80 symbols
# file by a Forth program called by Makefile.

# Note: The codes of `LOAD` (239) and `CODE` (175) are embedded in
# line 1, otherwise they would be parsed as text because of the `REM`.

1 CLEAR VAL "26111": RANDOMIZE USR VAL "15619": REM :\{239}"solo.bin"\{175}26112
2 RANDOMIZE USR VAL "26112": REM cold
3 RANDOMIZE USR VAL"26115": REM warm

# vim: ft=sinclairbasic
