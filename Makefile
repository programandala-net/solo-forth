# F+D Forth Makefile

# Copyright (C) 2015 By Marcos Cruz (programandala.net)

# http://programandala.net/en.program.f+d_forth.html

# Copying and distribution of this file, with or without modification, are
# permitted in any medium without royalty provided the copyright notice and
# this notice are preserved.  This file is offered as-is, without any warranty.

################################################################
# Requirements

# Vim (by Bram Moolenaar)
# 	http://vim.org

# head, cat and sort (from the GNU coreutils)

# bas2tap (by Martijn van der Heide)
#   Utilities section of
#   http://worldofspectrum.org

# Pasmo (by Julián Albo)
#   http://pasmo.speccy.org/

# fsb (by Marcos Cruz)
# 	http://programandala.net/en.program.fsb.html

################################################################
# Change history

# See at the end of the file.

################################################################
# Config

VPATH = ./

MAKEFLAGS = --no-print-directory

.ONESHELL:

################################################################
# Main

all: f+d_forth.tap

.PHONY : clean
clean:
	rm -f f+d_forth.bin.tap f+d_forth_loader.tap 

################################################################
# The binary

# The new basic loader.

f+d_forth_loader.tap: f+d_forth_loader.bas
	bas2tap -q -n -sAutoload -a1 -sForth \
		f+d_forth_loader.bas  \
		f+d_forth_loader.tap

# The new binary.

# A temporary name "forth.bin" is used because Pasmo does not have an option to
# choose the filename used in the TAP file header; it uses the name of the
# target file.

f+d_forth.bin.tap: f+d_forth.z80s
	cd src ; \
	pasmo --tap \
		f+d_forth.z80s \
		forth.bin \
		f+d_forth.symbols.z80s ; \
	mv forth.bin f+d_forth.bin.tap

# The new complete TAP file, ready to be loaded by the emulator.

f+d_forth.tap: \
	f+d_forth_loader.tap \
	f+d_forth.bin.tap
	cat f+d_forth_loader.tap f+d_forth.bin.tap > \
		f+d_forth.tap \

################################################################
# Backup

.PHONY: backup
backup:
	tar -czf backups/$$(date +%Y%m%d%H%M)_f+d_forth.tgz \
		Makefile \
		*.adoc \
		*.bas \
		*.z80s

################################################################
# Change history

# 2015-06-02: Start.
