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
object_file = solo_forth.o
#kernel_source_file = solo_forth.z80s.201506290300.z80s
#disk_source_file = solo_forth.fsb.201506290224.fsb

origin = 0x5E00

z80_dir=/usr/bin/
z80_prefix=z80-unknown-coff-
z80=$(z80_dir)$(z80_prefix)

as=$(z80)as
ld=$(z80)ld
nm=$(z80)nm

#ld_script = solo_forth.ld

.ONESHELL:

################################################################
# Main

.PHONY: all
all: solo_forth_disk_1.mgt solo_forth_disk_2.mgt

.PHONY: symbols
symbols: solo_forth.symbols.abs.txt

.PHONY : clean
clean:
	rm -f \
		solo_forth_disk_?.mgt \
		solo_forth.*.tap

################################################################
# The basic loader

solo_forth.bas.tap: solo_forth.bas
	bas2tap -q -n -sAutoload -a1 \
		solo_forth.bas  \
		solo_forth.bas.tap

################################################################
# The charset

# XXX OLD
# solo_forth.charset.bin: solo_forth.charset.z80s
# 	pasmo -v \
# 		solo_forth.charset.z80s \
# 		solo_forth.charset.bin

################################################################
# The object file

$(object_file): $(kernel_source_file) $(ld_script)
	$(as) \
		-z80 \
		-aglhs=solo_forth.list.txt \
		-o $(object_file) \
		$(kernel_source_file)

################################################################
# The binary file

# The filename "forth.bin" is used because bin2code does not
# have an option to choose the filename used in the TAP file
# header; it uses the name of the original file.

forth.bin: $(object_file)
	$(ld) \
		--trace \
		--oformat binary \
		-Map solo_forth.map \
		-Ttext=$(origin) \
		-Tdata=0xC000 \
		--output forth.bin \
		$(object_file)

# XXX with the script, the data section starts after the text section:
#		-T$(ld_script) \
# XXX same with this notation
#		--section-start=text=$(origin) \
#		--section-start=data=0xC000 \
# XXX this notation works!:
#		-Ttext=$(origin) \
#		-Tdata=0xC000 \

################################################################
# The TAP file

solo_forth.bin.tap: forth.bin
	bin2code forth.bin solo_forth.bin.tap $(origin)

################################################################
# The symbols file

# The GNU binutils `as` assembler prints the symbols only with
# relative values, because they are not linke yet. But the `ld`
# linker does not have an option to print them.
#
# The chosen solution is to get a symbols list from the object
# file using the `nm` utility, also from GNU binutils. The
# values are relative, but the format of the listing makes the
# file directly interpretable by a Forth program...

solo_forth.symbols.rel.txt: $(object_file)
	$(nm) $(object_file) > solo_forth.symbols.rel.txt

solo_forth.symbols.abs.txt: solo_forth.symbols.rel.txt
	gforth \
		nm2absolute.fs \
		solo_forth.symbols.rel.txt \
		-e bye \
		| sort > solo_forth.symbols.abs.txt

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
