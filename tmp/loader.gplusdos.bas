# gplusdos.bas

# Solo Forth loader for G+DOS

# This file is part of Solo Forth
# http://programandala.net/en.program.solo_forth.html

# Note: The symbols `24319`, `24320`, `24320` and `24323`
# are converted to their actual values, extracted from the Z80 symbols
# file by a Forth program called by Makefile.

1 CLEAR VAL "24319": LOAD d*"forth.bin" CODE VAL"24320"
2 POKE@VAL"10",NOT PI:RANDOMIZE USR VAL "24320": REM cold
3 RANDOMIZE USR VAL"24323": REM warm

# vim: ft=sinclairbasic
