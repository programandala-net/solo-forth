# Solo Forth Makefile

# Copyright (C) 2015 By Marcos Cruz (programandala.net)

# http://programandala.net/en.program.solo_forth.html

# Copying and distribution of this file, with or without modification, are
# permitted in any medium without royalty provided the copyright notice and
# this notice are preserved.  This file is offered as-is, without any warranty.

################################################################
# Requirements

# head, cat and sort (from the GNU coreutils)

# bas2tap (by Martijn van der Heide)
#   Utilities section of
#   http://worldofspectrum.org

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

disk_source_file = solo_forth.fsb
#kernel_source_file = solo_forth.z80s.201506290300.z80s
#disk_source_file = solo_forth.fsb.201506290224.fsb

origin = 0x5E00

#ld_script = solo_forth.ld

.ONESHELL:

################################################################
# Main

.PHONY: all
all: solo_forth_disk_1.mgt solo_forth_disk_2.mgt symbols

.PHONY: symbols
symbols: solo_forth.symbols.txt

.PHONY : clean
clean:
	rm -f \
		solo_forth_disk_?.mgt \
		solo_forth.*.tap

include Makefile.pasmo
#include Makefile.binutils

# XXX TODO
# .PHONY : pasmo
# pasmo:
# 	@make Makefile.pasmo
# .PHONY : as
# as:
# 	@make Makefile.binutils

################################################################
# The basic loader

# solo_forth.bas.tap: solo_forth.bas
# 	bas2tap -q -n -sAutoload -a1 \
# 		solo_forth.bas  \
# 		solo_forth.bas.tap

solo_forth.bas.tap: solo_forth.bas
	zmakebas -n Autoload -a 1 \
		-o solo_forth.bas.tap \
		solo_forth.bas

################################################################
# The charset

# XXX OLD
# solo_forth.charset.bin: solo_forth.charset.z80s
# 	pasmo -v \
# 		solo_forth.charset.z80s \
# 		solo_forth.charset.bin

################################################################
# The MGT disk images

# Disk 1 (for drive 1) contains G+DOS and the Forth system. The
# user will use disk 1 for customized versions of the Forth
# system, Forth turnkey applications, graphics and data files.

solo_forth_disk_1.mgt: solo_forth.bas.tap solo_forth.bin.tap
	mkmgt  solo_forth_disk_1.mgt \
		sys/gplusdos-sys-2a.tap \
		solo_forth.bas.tap \
		fzx/*.fzx \
		solo_forth.bin.tap

# Disk 2 (for drive 2) contains the source blocks of the Forth
# system, with a library of extensions and tools.  The disk
# sectors are used directly, without the G+DOS file system, the
# way classic Forth worked. The user can edit the original fsb
# source file with a modern editor and convert it to a fake MGT
# disk image with `mkmgt`, as shown:

mgt_file = $(basename $(disk_source_file) ).mgt

solo_forth_disk_2.mgt: $(disk_source_file)
	fsb2mgt $(disk_source_file) ;\
	mv $(mgt_file) solo_forth_disk_2.mgt

################################################################
# Backup

.PHONY: backup
backup:
	tar -cJf backups/$$(date +%Y%m%d%H%M)_solo_forth.tar.xz \
		Makefile* \
		*.adoc \
		_old/* \
		_ideas/* \
		_draft/* \
		_tests/* \
		inc/* \
		*.fs \
		*.sh \
		solo_forth*.fsb \
		solo_forth*.bas \
		solo_forth*.ld \
		solo_forth*.txt \
		solo_forth*.mgt \
		solo_forth*.z80s

################################################################
# Change history

# 2015-06-02: Start.
#
# 2015-06-17: Improved backup recipe.
#
# 2015-06-29: Improvement: The source filenames are
# configurable. This makes it easier to try old versions, for
# debugging.
#
# 2015-07-22: Added the MGT disk images to the backup. Sometimes
# it's useful to test and old version without recompiling the
# old sources.
#
# 2015-08-14: Updated backup recipe.
#
# 2015-08-17: Modified to use GNU binutils instead of Pasmo.
#
# 2015-08-18: Improved. New: a Forth program creates the symbols
# file.
#
# 2015-08-20: Divided in three parts: Makefile, Makefile.pasmo, Makefile.binutils.
