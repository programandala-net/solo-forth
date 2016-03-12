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

# bin2code (by Metalbrain)
# 	http://metalbrain.speccy.org/link-eng.htm

# fsb2 (by Marcos Cruz)
# 	http://programandala.net/en.program.fsb2.html

# mkmgt (by Marcos Cruz)
# 	http://programandala.net/en.program.mkmgt.html

################################################################
# Change history

# See at the end of the file.

################################################################
# Config

VPATH = ./

MAKEFLAGS = --no-print-directory

disk_source_file = library.fsb
#kernel_source_file = solo_forth.z80s.201506290300.z80s
#disk_source_file = library.fsb.201506290224.fsb

origin = 0x5E00

#ld_script = solo_forth.ld

.ONESHELL:

################################################################
# Main

# XXX OLD -- tests
# .PHONY: test
# test: test2
# 	echo $(pattern)
# 	ls $(pattern)
# .PHONY: test2
# test2:
# 	$(eval pattern=ker*)
# 	echo variable pattern is $(pattern)

.PHONY: all
all: gplusdos
#all: gplusdos plus3dos

.PHONY: gplusdos
gplusdos: solo_forth_disk_1.mgt solo_forth_disk_2.mgt

.PHONY: plus3dos
plus3dos: solo_forth_disk_a.dsk
# plus3dos: solo_forth_disk_a.dsk solo_forth_disk_b.dsk

# XXX OLD
# .PHONY: setgplusdos
# setgplusdos:
# 	$(eval dos=gplusdos)
# .PHONY: setplus3dos
# setplus3dos:
# 	$(eval dos=plus3dos)

# XXX OLD -- all included simbols
#	symbols

# XXX OLD
#.PHONY: symbols
#symbols: solo_forth.symbols.txt

.PHONY : clean
clean:
	rm -f \
		solo_forth_disk_?.mgt \
		solo_forth_disk_?.dsk \
		solo_forth.*.tap \
		sys/4x8fd.tap \
		sys/prnt42.tap

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

# XXX OLD
# loader.bas.tap: loader.bas
# 	bas2tap -q -n -sAutoload -a1 \
# 		loader.bas  \
# 		loader.bas.tap

# XXX OLD
# loader.bas.tap: loader.bas
# 	zmakebas -n Autoload -a 1 \
# 		-o loader.bas.tap \
# 		loader.bas

# XXX OLD
# loader.bas.tap: loader.bas
# 	zmakebas -n $(if gplusdos,Autoload,DISK) -a 1 \
# 		-o loader.$(dos).bas.tap \
# 		loader.$(dos).bas

loader.gplusdos.bas.tap: loader.gplusdos.bas
	zmakebas -n Autoload -a 1 \
		-o loader.gplusdos.bas.tap \
		loader.gplusdos.bas

loader.plus3dos.bas.tap: loader.plus3dos.bas
	zmakebas -n DISK -a 1 \
		-o loader.plus3dos.bas.tap \
		loader.plus3dos.bas

################################################################
# The charset

# XXX OLD
# solo_forth.charset.bin: solo_forth.charset.z80s
# 	pasmo -v \
# 		solo_forth.charset.z80s \
# 		solo_forth.charset.bin

################################################################
# Font drivers

# This drivers are not part of the Solo Forth library yet.
# Meanwhile, their binary files are included in the first disk.

# Note: an intermediate file called "4x8fd.bin" is used, in
# order to force that filename in the TAP header and therefore
# in the disk image.

sys/4x8fd.tap: sys/4x8fd.z80s
	cd sys ; \
	pasmo 4x8fd.z80s 4x8fd.bin ; \
	bin2code 4x8fd.bin 4x8fd.tap 60000 ; \
	cd - 

sys/prnt42.tap: sys/prnt42.bin
	cd sys ; \
	bin2code prnt42.bin prnt42.tap 63610 ; \
	cd - 

################################################################
# Fonts

# The DSK disk image needs these binary files to be converted to
# TAP first.

fzx_fonts = $(wildcard fzx/*.fzx)

fzx/fzx_fonts.tap : $(fzx_fonts)
	cd fzx ; \
	for file in $$(ls -1 *.fzx); do \
		bin2code $$file $$file.tap; \
	done; \
	cat *.fzx.tap > fzx_fonts.tap ; \
	rm -f *.fzx.tap ; \
	cd -

################################################################
# The disk images

# The first disk ("1" for G+DOS, "A" for +3DOS) contains the
# Forth system. The user will use the first disk for customized
# versions of the Forth system, Forth turnkey applications,
# graphics and data files.

solo_forth_disk_1.mgt: \
		loader.gplusdos.bas.tap \
		kernel.gplusdos.bin.tap \
		sys/4x8fd.tap \
		sys/prnt42.tap
	mkmgt solo_forth_disk_1.mgt \
		sys/gplusdos-sys-2a.tap \
		loader.gplusdos.bas.tap \
		kernel.gplusdos.bin.tap \
		sys/ea5aky-font42.tap \
		sys/4x8fd.tap \
		sys/prnt42.tap \
		fzx/*.fzx

tmp/solo_forth_disk_a.tap: \
		loader.plus3dos.bas.tap \
		kernel.plus3dos.bin.tap \
		sys/4x8fd.tap \
		sys/prnt42.tap \
		fzx/fzx_fonts.tap
	cat \
		loader.plus3dos.bas.tap \
		kernel.plus3dos.bin.tap \
		sys/ea5aky-font42.tap \
		sys/4x8fd.tap \
		sys/prnt42.tap \
		fzx/fzx_fonts.tap \
		> tmp/solo_forth_disk_a.tap

# XXX OLD
# solo_forth_disk_a.dsk: \
# 		sys/4x8fd.tap \
# 		loader.plus3dos.bas.tap \
# 		kernel.plus3dos.bin.tap
# 	mkp3fs -720  -label SoloForth \
# 		solo_forth_disk_a.dsk \
# 		loader.plus3dos.bas.tap \
# 		kernel.bin.tap \
# 		sys/ea5aky-font42.tap \
# 		sys/4x8fd.tap \
# 		sys/print-42-bin.tap \
# 		fzx/*.fzx

solo_forth_disk_a.dsk: tmp/solo_forth_disk_a.tap
	tap2dsk -720 -label SoloForth \
		tmp/solo_forth_disk_a.tap \
		solo_forth_disk_a.dsk

# The second disk ("2" for G+DOS, "B" for +3DOS) contains the
# source blocks of the Forth system.  The blocks are stored on
# the disk sectors, without file system.

library.complete.for_gplusdos.fsb: \
	library.main.fsb \
	library.game.nuclear_invaders.fsb \
	library.error_codes.gplusdos.fsb \
	library.error_codes.os.fsb
	cat \
		library.main.fsb \
		library.game.nuclear_invaders.fsb \
		library.error_codes.gplusdos.fsb \
		library.error_codes.os.fsb \
		> library.complete.for_gplusdos.fsb

solo_forth_disk_2.mgt: library.complete.for_gplusdos.fsb
	fsb2-mgt library.complete.for_gplusdos.fsb ;\
	mv library.complete.for_gplusdos.mgt solo_forth_disk_2.mgt

library.complete.for_plus3dos.fsb: \
	library.main.fsb \
	library.game.nuclear_invaders.fsb \
	library.error_codes.plus3dos.fsb \
	library.error_codes.os.fsb
	cat \
		library.main.fsb \
		library.error_codes.plus3dos.fsb \
		library.game.nuclear_invaders.fsb \
		library.error_codes.os.fsb \
		> library.complete.for_plus3dos.fsb

# XXX TODO -- `fsb2-dsk` is not ready yet.
solo_forth_disk_b.dsk: library.complete.for_plus3dos.fsb
	fsb2-dsk library.complete.for_plus3dos.fsb ;\
	mv library.complete.for_plus3dos.dsk solo_forth_disk_b.dsk

################################################################
# Backup

.PHONY: backup
backup:
	tar -cJf backups/$$(date +%Y%m%d%H%M)_solo_forth.tar.xz \
		_draft/* \
		_old/* \
		_tests/* \
		inc/* \
		Makefile* \
		*.adoc \
		*.bas \
		*.fsb \
		*.mgt \
		*.dsk \
		*.sh \
		*.txt \
		*.z80s

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
#
# 2015-10-10: Substituted fsb (written in Vim) with fsb2
# (written in Forth for Gforth). fsb was becoming too show with
# more than 300 source screens, while fsb2 is instantaneous.
#
# 2015-10-15: Updated.
#
# 2015-11-10: First changes to support also the +3DOS version.
#
# 2015-11-11: The DOS error codes are separated from the main file of the library.
#
# 2015-11-12: Fixed the load address of the font drivers; they
# were missing because of the recent use of `bin2code`, required
# to build +3DOS disk images.
#
# 2016-02-15: Added Nuclear Invaders to the library, an
# game under development for Solo Forth, in order to try it.
