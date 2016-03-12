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

# mkmgt (by Marcos Cruz)
# 	http://programandala.net/en.program.mkmgt.html

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

all: f+d_forth_disk_1.mgt f+d_forth_disk_2.mgt

.PHONY : clean
clean:
	rm -f \
		f+d_forth_disk_?.mgt \
		f+d_forth.*.tap


################################################################
# The binary

# The new basic loader.

f+d_forth.bas.tap: f+d_forth.bas
	bas2tap -q -n -sAutoload -a1 \
		f+d_forth.bas  \
		f+d_forth.bas.tap

# The new binary.

# A temporary name "forth.bin" is used because Pasmo does not have an option to
# choose the filename used in the TAP file header; it uses the name of the
# target file.

f+d_forth.bin.tap: f+d_forth.z80s
	pasmo --tap \
		f+d_forth.z80s \
		forth.bin \
		f+d_forth.symbols.z80s ; \
	mv forth.bin f+d_forth.bin.tap

################################################################
# The MGT disk images

# Disk 1 (for drive 1) contains G+DOS and the Forth system. The user
# will use disk 1 for customized versions of the Forth system, Forth
# applications and will their data files.

f+d_forth_disk_1.mgt: f+d_forth.bas.tap f+d_forth.bin.tap
	mkmgt  f+d_forth_disk_1.mgt \
		sys/gplusdos-sys-2a.tap \
		f+d_forth.bas.tap \
		f+d_forth.bin.tap

# Disk 2 (for drive 2) contains the source blocks of the Forth system,
# with a library of extensions and tools.  The disk sectors are used
# directly, without the G+DOS file system, the way classic Forth
# worked. The user can edit the original fsb source file with a modern
# editor, in order to write Forth programs, and then use `mkmgt` to
# convert it to a fake MGT disk image, as shown:

f+d_forth_disk_2.mgt: f+d_forth.fsb
	fsb2mgt f+d_forth.fsb ;\
	mv f+d_forth.mgt f+d_forth_disk_2.mgt

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
