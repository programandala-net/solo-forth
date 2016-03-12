# Solo Forth Makefile

# Copyright (C) 2015 By Marcos Cruz (programandala.net)

# http://programandala.net/en.program.solo_forth.html

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

# bin2code (by Metalbrain)
# 	http://metalbrain.speccy.org/link-eng.htm

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

kernel_source_file = solo_forth.z80s
disk_source_file = solo_forth.fsb
#kernel_source_file = solo_forth.z80s.201506290300.z80s
#disk_source_file = solo_forth.fsb.201506290224.fsb

origin = 0x5E00
ld_script = solo_forth.ld

.ONESHELL:

################################################################
# Main

.PHONY: all
all: solo_forth_disk_1.mgt solo_forth_disk_2.mgt

.PHONY : clean
clean:
	rm -f \
		solo_forth_disk_?.mgt \
		solo_forth.*.tap


################################################################
# The binary

# The new basic loader.

solo_forth.bas.tap: solo_forth.bas
	bas2tap -q -n -sAutoload -a1 \
		solo_forth.bas  \
		solo_forth.bas.tap

# The charset.

# XXX OLD
# solo_forth.charset.bin: solo_forth.charset.z80s
# 	pasmo -v \
# 		solo_forth.charset.z80s \
# 		solo_forth.charset.bin

# The new binary.

# A temporary name "forth.bin" is used because Pasmo does not have an option to
# choose the filename used in the TAP file header; it uses the name of the
# target file.

solo_forth.bin.tap: $(kernel_source_file) $(ld_script)
	/usr/bin/z80-unknown-coff-as \
		-z80 \
		-aglhs=solo_forth.list.txt \
		-o solo_forth.o \
		$(kernel_source_file) && \
	/usr/bin/z80-unknown-coff-ld \
		--trace \
		--oformat binary \
		-Map solo_forth.map \
		-Ttext=$(origin) \
		-Tdata=0xC000 \
		--output forth.bin \
		solo_forth.o && \
	bin2code forth.bin solo_forth.bin.tap $(origin)

# XXX with the script, the data section starts after the text section:
#		-T$(ld_script) \
# XXX same with this notation
#		--section-start=text=$(origin) \
#		--section-start=data=0xC000 \
# XXX this notation works!:
#		-Ttext=$(origin) \
#		-Tdata=0xC000 \

# > solo_forth.assembly_debug_info.txt ; \

# solo_forth.bin.tap: $(kernel_source_file)
# 	z80asm \
# 		--input=$(kernel_source_file) \
# 		--output=forth.bin \
# 		--label=solo_forth.symbols.z80asm.z80s \
# 		--list=solo_forth.list.txt \
# 	bin2tap forth.bin solo_forth.bin.tap

################################################################
# The MGT disk images

# Disk 1 (for drive 1) contains G+DOS and the Forth system. The
# user will use disk 1 for customized versions of the Forth
# system, Forth turnkey applications, graphics and data files.

solo_forth_disk_1.mgt: solo_forth.bas.tap solo_forth.bin.tap
	mkmgt  solo_forth_disk_1.mgt \
		sys/gplusdos-sys-2a.tap \
		solo_forth.bas.tap \
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
		Makefile \
		*.adoc \
		_old/* \
		_ideas/* \
		_draft/* \
		solo_forth*.fsb \
		solo_forth*.bas \
		solo_forth*.ld \
		solo_forth*.sh \
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
