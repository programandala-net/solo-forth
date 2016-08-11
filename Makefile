# Makefile

# This file is part of Solo Forth
# http://programandala.net/en.program.solo_forth.html

# Last modified: 201608111558

# ==============================================================
# Author

# Marcos Cruz (programandala.net), 2015, 2016.

# ==============================================================
# License

# You may do whatever you want with this work, so long as you
# retain every copyright, credit and authorship notice, and this
# license.  There is no warranty.

# ==============================================================
# Requirements

# head, cat and sort (from the GNU coreutils)

# zmakebas (by Russell Marks)
#   Usually included in Linux distros. Also see:
# 	http://sourceforge.net/p/emuscriptoria/code/HEAD/tree/desprot/ZMakeBas.c
# 	https://github.com/catseye/zmakebas
# 	http://zmakebas.sourcearchive.com/documentation/1.2-1/zmakebas_8c-source.html

# bin2code (by Metalbrain)
# 	http://metalbrain.speccy.org/link-eng.htm

# fsb2 (by Marcos Cruz)
# 	http://programandala.net/en.program.fsb2.html

# mkmgt (by Marcos Cruz)
# 	http://programandala.net/en.program.mkmgt.html

# Forth Foundation Library (by Dick van Oudheusden)
# 	http://irdvo.github.io/ffl/

# DOSBox (by The DOSBox Team)
#   http://www.dosbox.com

# ==============================================================
# History

# See at the end of the file.

# ==============================================================
# To-do

# XXX FIXME -- the loader and the disk image are built even when the
# sources are update,

# ==============================================================
# Notes

# $@ the name of the target of the rule
# $< the name of the first prerequisite
# $? the names of all the prerequisites that are newer than the target
# $^ the names of all the prerequisites

# ==============================================================
# Config

VPATH = ./

MAKEFLAGS = --no-print-directory

#.ONESHELL:

# ==============================================================
# Main

.PHONY: all
all: gplusdos trdos plus3dos

.PHONY: gplusdos
gplusdos: \
	disks/gplusdos/disk0.mgt \
	disks/gplusdos/disk1_lib.mgt \
	disks/gplusdos/disk2_lib+games.mgt \
	disks/gplusdos/disk3_lib+benchmarks.mgt \
	disks/gplusdos/disk4_lib+tests.mgt

.PHONY: plus3dos
plus3dos: \
	disks/plus3dos/disk0.dsk
# disks/plus3dos/disk1_lib.dsk
# XXX TODO --

.PHONY: trdos
trdos: \
	disks/trdos/disk0.trd \
	disks/trdos/disk1_lib.trd \
	disks/trdos/disk2_lib+games.trd \
	disks/trdos/disk3_lib+benchmarks.trd \
	disks/trdos/disk4_lib+tests.trd

.PHONY: disk9
disk9: \
	disks/gplusdos/disk9_lib_without_dos.mgt \
	disks/trdos/disk9_lib_without_dos.trd

# Note: disk9 is a special disk image used for debugging. It contains
# the core library except the DOS modules. This means disk9 contains
# exactly the same blocks in all DOS implementations. This is useful
# to check if disk access works fine when a new DOS is implemented,
# comparing the output of `blks` (a debug tool, temporarily included
# in the kernel) to a different DOS.

.PHONY: clean
clean: cleantmp cleandisks

.PHONY: cleandisks
cleandisks: cleangplusdos cleanplus3dos cleantrdos

.PHONY: cleangplusdos
cleangplusdos:
	rm -f disks/gplusdos/*.mgt

.PHONY: cleanplus3dos
cleanplus3dos:
	rm -f disks/plus3dos/*.dsk

.PHONY: cleantrdos
cleantrdos:
	rm -f disks/trdos/*.trd

.PHONY: cleantmp
cleantmp:
	-rm -f tmp/*

include Makefile.pasmo

# XXX OLD
# .PHONY : pasmo
# pasmo:
# 	@make Makefile.pasmo
# .PHONY : as
# as:
# 	@make Makefile.binutils

# ==============================================================
# Debug

# ==============================================================
# Loader

# The BASIC loader of the system is coded in plain text. The addresses
# that depend on the kernel (its load adresses and entry points) are
# represented in the source by labels. A Forth program replaces the
# labels with the actual values, extracted from the symbols file
# created by the assembler. Then zmakebas converts the patched loader
# into a TAP file, ready to be copied to a disk image.

# ----------------------------------------------
# G+DOS loader

tmp/loader.gplusdos.bas: \
	tmp/kernel.symbols.gplusdos.z80s \
	src/loader/gplusdos.bas
	gforth tools/patch_the_loader.fs $^ $@

tmp/loader.gplusdos.bas.tap: tmp/loader.gplusdos.bas
	zmakebas -n Autoload -a 1 -o $@ $<

# ----------------------------------------------
# +3DOS loader

tmp/loader.plus3dos.bas: \
	tmp/kernel.symbols.plus3dos.z80s \
	src/loader/plus3dos.bas
	gforth tools/patch_the_loader.fs $^ $@

tmp/loader.plus3dos.bas.tap: tmp/loader.plus3dos.bas
	zmakebas -n DISK -a 1 -o $@ $<

# ----------------------------------------------
# TR-DOS loader

tmp/loader.trdos.bas: \
	tmp/kernel.symbols.trdos.z80s \
	src/loader/trdos.bas
	gforth tools/patch_the_loader.fs $^ $@

tmp/loader.trdos.bas.tap: tmp/loader.trdos.bas
	zmakebas -n boot -a 1 -o $@ $<

# ==============================================================
# Font drivers

# These drivers are not part of the Solo Forth library yet.
# Meanwhile, their binary files are included in the first disk.

# Note: an intermediate file called "4x8fd.bin" is used, in
# order to force that filename in the TAP header and therefore
# in the disk image.  This file must be in the current
# directory, because the path specified in the command will be
# part of the filename in the TAP header.

# XXX WARNING -- 2016-03-19. bin2code returns error 97 when one
# the filenames has a path, but it creates the tap file as
# usual.  A hyphen at the beginning of the recipe line forces
# make to ignore the error.

tmp/4x8fd.tap: src/modules/4x8fd.z80s
	pasmo --tap $< 4x8fd.bin ; \
	mv 4x8fd.bin $@

tmp/prnt42.tap: bin/modules/prnt42.bin
	cd bin/modules/ ; \
	bin2code prnt42.bin prnt42.tap 63610 ; \
	cd - ; \
	mv bin/modules/prnt42.tap tmp/prnt42.tap

# ==============================================================
# Fonts

# The DSK disk image needs the fzx files to be packed into TAP
# first.

fzx_fonts = $(wildcard bin/fonts/*.fzx)

tmp/fzx_fonts.tap : $(fzx_fonts)
	cd bin/fonts ; \
	for file in $(notdir $(fzx_fonts)); do \
		bin2code $$file $$file.tap; \
	done; \
	cd -
	cat $(addsuffix .tap,$(fzx_fonts)) > $@ ; \
	rm -f $(addsuffix .tap, $(fzx_fonts)) 

# ==============================================================
# Main disk

# The first disk contains the Forth system. The user will use the
# first disk for customized versions of the Forth system, Forth
# turnkey applications, fonts, graphics and data.

# ----------------------------------------------
# G+DOS main disk

disks/gplusdos/disk0.mgt: \
		tmp/loader.gplusdos.bas.tap \
		tmp/kernel.gplusdos.bin.tap \
		tmp/4x8fd.tap \
		bin/fonts/ea5aky-font42.tap \
		tmp/prnt42.tap \
		tmp/fzx_fonts.tap
	mkmgt $@ bin/sys/gplusdos-sys-2a.tap $^

# ----------------------------------------------
# +3DOS main disk

tmp/disk0.plus3dos.tap: \
		tmp/loader.plus3dos.bas.tap \
		tmp/kernel.plus3dos.bin.tap \
		tmp/4x8fd.tap \
		bin/fonts/ea5aky-font42.tap \
		tmp/prnt42.tap \
		tmp/fzx_fonts.tap
	cat $^ > $@

disks/plus3dos/disk0.dsk: tmp/disk0.plus3dos.tap
	tap2dsk -720 -label SoloForth $< $@

# ----------------------------------------------
# TR-DOS main disk

tmp/disk0.trdos.tap: \
		tmp/loader.trdos.bas.tap \
		tmp/kernel.trdos.bin.tap \
		tmp/4x8fd.tap \
		bin/fonts/ea5aky-font42.tap \
		tmp/prnt42.tap \
		tmp/fzx_fonts.tap
	cat $^ > $@

disks/trdos/disk0.trd: tmp/disk0.trdos.tap
	cd tmp && ln -sf $(notdir $<) TRDOS-D0.TAP && cd -
	rm -f $@
	ln -f tools/emptytrd.exe tools/writetrd.exe tmp/
	cd tmp && \
	echo "EMPTYTRD.EXE SoloFth0.TRD" > mktrd.bat && \
	echo "WRITETRD.EXE SoloFth0.TRD TRDOS-D0.TAP" >> mktrd.bat && \
	dosbox -exit mktrd.bat && \
	cd -
	mv tmp/SOLOFTH0.TRD $@

# XXX TODO -- write a program in Gforth to create an empty TRD disk
# image, so the lowercase letters of the disk label are preserved.
# `emptytrd.exe` makes the label uppercase.

# ==============================================================
# Library disks

# The library disks contains the source blocks of the library.
# Depending on the DOS, the blocks are stored in a blocks file or
# directly in the disk sectors.

all_lib_files = $(sort $(wildcard src/lib/*.fsb))
dos_lib_files = $(sort $(wildcard src/lib/dos.*.fsb))
game_lib_files = $(sort $(wildcard src/lib/game.*.fsb))
meta_tools_lib_files = $(sort $(wildcard src/lib/meta.*.fsb))
benchmark_lib_files = $(sort $(wildcard src/lib/meta.benchmark*.fsb))
test_lib_files = $(sort $(wildcard src/lib/meta.test*.fsb))
core_lib_files = \
	$(filter-out $(game_lib_files) $(meta_tools_lib_files), \
			$(all_lib_files))
no_dos_core_lib_files = \
	$(filter-out $(dos_lib_files), $(core_lib_files))

tmp/library_without_dos.fsb: $(no_dos_core_lib_files)
	cat $(no_dos_core_lib_files) > $@

# ----------------------------------------------
# G+DOS library disks

# XXX WARNING -- `%` in filter patterns works only at the start
# of the pattern

gplusdos_core_lib_files = \
	$(filter-out %trdos.fsb %plus3dos.fsb %idedos.fsb , $(core_lib_files))

tmp/library_for_gplusdos.fsb: $(gplusdos_core_lib_files)
	cat $(gplusdos_core_lib_files) > $@

disks/gplusdos/disk1_lib.mgt: tmp/library_for_gplusdos.fsb
	fsb2-mgt tmp/library_for_gplusdos.fsb ;\
	mv tmp/library_for_gplusdos.mgt $@

tmp/library_for_gplusdos+games.fsb: $(gplusdos_core_lib_files) $(game_lib_files)
	cat $(gplusdos_core_lib_files) $(game_lib_files) > $@

disks/gplusdos/disk2_lib+games.mgt: tmp/library_for_gplusdos+games.fsb
	fsb2-mgt $< ;\
	mv tmp/library_for_gplusdos+games.mgt $@

tmp/library_for_gplusdos+benchmarks.fsb: $(gplusdos_core_lib_files) $(benchmark_lib_files)
	cat $(gplusdos_core_lib_files) $(benchmark_lib_files) > $@

disks/gplusdos/disk3_lib+benchmarks.mgt: tmp/library_for_gplusdos+benchmarks.fsb
	fsb2-mgt $< ;\
	mv tmp/library_for_gplusdos+benchmarks.mgt $@

tmp/library_for_gplusdos+tests.fsb: $(gplusdos_core_lib_files) $(test_lib_files)
	cat $(gplusdos_core_lib_files) $(test_lib_files) > $@

disks/gplusdos/disk4_lib+tests.mgt: tmp/library_for_gplusdos+tests.fsb
	fsb2-mgt $< ;\
	mv tmp/library_for_gplusdos+tests.mgt $@

disks/gplusdos/disk9_lib_without_dos.mgt: tmp/library_without_dos.fsb
	fsb2-mgt $< ;\
	mv tmp/library_without_dos.mgt $@

# XXX TODO -- convert games to FBA
# tmp/nuclear_invaders.fba: src/nuclear_invaders.fs
# 	./make/fs2fba.sh $< ;\
# 	mv $(basename $<).fba $@

# ----------------------------------------------
# +3DOS library disks

# XXX UNDER DEVELOPMENT

plus3dos_core_lib_files = $(filter-out %trdos.fsb %gplusdos.fsb,$(core_lib_files))

tmp/library_for_plus3dos.fsb: $(plus3dos_core_lib_files) $(meta_tools_lib_files)
	cat \
		$(gplusdos_core_lib_files) $(meta_tools_lib_files) \
		> tmp/library_for_plus3dos.fsb

# XXX TODO -- `fsb2-dsk` is not ready yet.
disks/plus3dos/disk1.dsk: tmp/library_for_plus3dos.fsb
	fsb2-dsk tmp/library_for_plus3dos.fsb ;\
	mv tmp/library_for_plus3dos.dsk disk1.dsk

# ----------------------------------------------
# TR-DOS library disks

trdos_core_lib_files = \
	$(filter-out %gplusdos.fsb %plus3dos.fsb %idedos.fsb , $(core_lib_files))

tmp/library_for_trdos.fsb: $(trdos_core_lib_files)
	cat $(trdos_core_lib_files) > $@

disks/trdos/disk1_lib.trd: tmp/library_for_trdos.fsb 
	fsb2-trd $< SoloFth1 ; \
	mv $(basename $<).trd $@

tmp/library_for_trdos+games.fsb: \
	$(trdos_core_lib_files) $(game_lib_files)
	cat $(trdos_core_lib_files) $(game_lib_files) > $@

disks/trdos/disk2_lib+games.trd: tmp/library_for_trdos+games.fsb
	fsb2-trd $< SoloFth2 ; \
	mv $(basename $<).trd $@

tmp/library_for_trdos+benchmarks.fsb: \
	$(trdos_core_lib_files) $(benchmark_lib_files)
	cat $(trdos_core_lib_files) $(benchmark_lib_files) > $@

disks/trdos/disk3_lib+benchmarks.trd: tmp/library_for_trdos+benchmarks.fsb
	fsb2-trd $< SoloFth3 ; \
	mv $(basename $<).trd $@

tmp/library_for_trdos+tests.fsb: \
	$(trdos_core_lib_files) $(test_lib_files)
	cat $(trdos_core_lib_files) $(test_lib_files) > $@

disks/trdos/disk4_lib+tests.trd: tmp/library_for_trdos+tests.fsb
	fsb2-trd $< SoloFth4 ; \
	mv $(basename $<).trd $@

disks/trdos/disk9_liblib_without_dos.trd: tmp/library_without_dos.fsb
	fsb2-trd $< SoloFth9 ; \
	mv $(basename $<).trd $@

# ==============================================================
# Backup

.PHONY: backupsrc
backupsrc:
	tar -cJf backups/$$(date +%Y%m%d%H%M)_solo_forth_src.tar.xz \
		Makefile* \
		src/lib/*.fsb \
		src/kernel.z80s

.PHONY: backuplib
backuplib:
	tar -cJf backups/$$(date +%Y%m%d%H%M)_solo_forth_lib.tar.xz \
		src/lib/*.fsb

# XXX TODO --
# .PHONY: backupkernel
# backupkernel:
# 	xz src/kernel.z80s
# 	mv src/kernel.z80s.xz backups/$$(date +%Y%m%d%H%M)_solo_forth_kernel.z80s.xz \

# XXX OLD
.PHONY: oldbackup
oldbackup:
	tar -cJf backups/$$(date +%Y%m%d%H%M)_solo_forth.tar.xz \
		_draft/* \
		_old/* \
		_tests/* \
		src/* \
		Makefile* \
		*.adoc \
		*.mgt \
		*.dsk \
		*.sh \
		*.txt

# ==============================================================
# History

# 2015-06-02: Start.
#
# 2015-06-17: Improved backup recipe.
#
# 2015-06-29: Improvement: The source filenames are configurable. This
# makes it easier to try old versions, for debugging.
#
# 2015-07-22: Added the MGT disk images to the backup.  Sometimes it's
# useful to test and old version without recompiling the old sources.
#
# 2015-08-14: Updated backup recipe.
#
# 2015-08-17: Modified to use GNU binutils instead of Pasmo.
#
# 2015-08-18: Improved. New: a Forth program creates the symbols file.
#
# 2015-08-20: Divided in three parts: Makefile, Makefile.pasmo,
# Makefile.binutils.
#
# 2015-10-10: Substituted fsb (written in Vim) with fsb2 (written in
# Forth for Gforth). fsb was becoming too show with more than 300
# source screens, while fsb2 is instantaneous.
#
# 2015-10-15: Updated.
#
# 2015-11-10: First changes to support also the +3DOS version.
#
# 2015-11-11: The DOS error codes are separated from the main file of
# the library.
#
# 2015-11-12: Fixed the load address of the font drivers; they were
# missing because of the recent use of `bin2code`, required to build
# +3DOS disk images.
#
# 2016-02-15: Added Nuclear Invaders to the library, a game under
# development for Solo Forth, in order to try it.
#
# 2016-02-22: Delete also <library.complete.*.fsb> in clean.  It seems
# this is required when parts of the library are simbolyc links.
#
# 2016-03-19: Updated after the reorganization of files into
# directories.
#
# 2016-03-22: Removed Nuclear Invaders from the library, because now
# it's built on its own directory.
#
# 2016-03-24: New partial backups.
#
# 2016-04-13: Improved: the BASIC loader is patched with the current
# memory addresses, extracted from the Z80 symbols file. Updated the
# requirements and the license.
#
# 2016-04-16: Fix: the patching of the loader didn't work after `make
# clean`, because a prerequisite was missing.
#
# 2016-04-16: Also the +3DOS loader is patched with the current values
# of the kernel.
#
# 2016-05-02: Make two library disks: the main one, now without games;
# a new one, with games but without the meta benchamarks/tests. This
# is needed, because the library has grown too much, and it's near the
# limit of a G+DOS disk (800 KiB).
#
# 2016-05-03: Make three library disks: 1) only with the core library;
# 2) the core library plus the meta tools (benchmarks and tests for
# Solo Forth itself); 3) the core library plus the sample games.
#
# 2016-08-03: Make first changes to support TR-DOS.
#
# 2016-08-04: Fix TRD library disks with a track 0.
#
# 2016-08-05: Create the disks images in <disks/DOSNAME>. Split the
# meta tools disk into two: tests and benchmarks. Rename all disk
# image files after a shorter and clearer format. Add disk9 for
# debugging.
#
# 2016-08-10: Activate disk0 of +3DOS.
#
# 2016-08-11: Make TRD disk images with the improved version of
# fsb2-trd, which now creates a sector 0, instead of the ad hoc
# solution.
#
# 2016-08-11: 
