# plus3dos.bas

# Solo Forth loader for +3DOS

# This file is part of Solo Forth
# http://programandala.net/en.program.solo_forth.html

# Note: The symbols `ramtop`, `origin`, `cold_entry` and `warm_entry`
# are converted to their actual values, extracted from the Z80 symbols
# file by a Forth program called by Makefile.

1 CLEAR VAL "ramtop": LOAD "forth.bin" CODE VAL"origin"
2 RANDOMIZE USR VAL "cold_entry": REM cold
3 RANDOMIZE USR VAL"warm_entry": REM warm

# vim: ft=sinclairbasic
