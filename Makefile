# Makefile

# This file is part of Solo Forth
# http://programandala.net/en.program.solo_forth.html

# Last modified: 201611260136

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

# bin2code (by Metalbrain)
# 	http://metalbrain.speccy.org/link-eng.htm

# cat (from the GNU coreutils)

# DOSBox (by The DOSBox Team)
#   http://www.dosbox.com

# Forth Foundation Library (by Dick van Oudheusden)
# 	http://irdvo.github.io/ffl/

# fsb2 (by Marcos Cruz)
# 	http://programandala.net/en.program.fsb2.html

# head (from the GNU coreutils)

# mkmgt (by Marcos Cruz)
# 	http://programandala.net/en.program.mkmgt.html

# sort (from the GNU coreutils)

# zmakebas (by Russell Marks)
#   Usually included in Linux distros. Also see:
# 	http://sourceforge.net/p/emuscriptoria/code/HEAD/tree/desprot/ZMakeBas.c
# 	https://github.com/catseye/zmakebas
# 	http://zmakebas.sourcearchive.com/documentation/1.2-1/zmakebas_8c-source.html

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
	disks/gplusdos/disk3_lib+misc_benchmarks.mgt \
	disks/gplusdos/disk4_lib+rng_benchmarks.mgt \
	disks/gplusdos/disk5_lib+flow_benchmarks.mgt \
	disks/gplusdos/disk6_lib+tests.mgt

.PHONY: plus3dos
plus3dos: \
	disks/plus3dos/disk0.180.dsk \
	disks/plus3dos/disk0.720.dsk \
	disks/plus3dos/disk1_lib.dsk \
	disks/plus3dos/disk2_lib+games.dsk \
	disks/plus3dos/disk3_lib+misc_benchmarks.dsk \
	disks/plus3dos/disk4_lib+rng_benchmarks.dsk \
	disks/plus3dos/disk5_lib+flow_benchmarks.dsk \
	disks/plus3dos/disk6_lib+tests.dsk

.PHONY: trdos
trdos: \
	disks/trdos/disk0.trd \
	disks/trdos/disk1_lib.trd \
	disks/trdos/disk2_lib+games.trd \
	disks/trdos/disk3_lib+misc_benchmarks.trd \
	disks/trdos/disk4_lib+rng_benchmarks.trd \
	disks/trdos/disk5_lib+flow_benchmarks.trd \
	disks/trdos/disk6_lib+tests.trd

.PHONY: disk9
disk9: \
	disks/gplusdos/disk9_lib_without_dos.mgt \
	disks/plu3sdos/disk9_lib_without_dos.dsk \
	disks/trdos/disk9_lib_without_dos.trd

# Note: disk9 is a special disk image used for debugging. It contains
# the core library except the DOS modules. This means disk9 contains
# exactly the same blocks in all DOS implementations. This is useful
# to check if disk access works fine when a new DOS is implemented,
# comparing the output of `blks` (a debug tool, temporarily included
# in the kernel).

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
	-rm -f tmp/[a-zA-Z0-9_]*
# Note: The file <tmp/.gitignore> must be preserved;
#       that's why the wildcard is used.

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
# that depend on the kernel (its load address and entry points) are
# represented in the source by labels. A Forth program replaces the
# labels with the actual values, extracted from the symbols file
# created by the assembler. Then zmakebas converts the patched loader
# into a TAP file, ready to be copied to a disk image.

# XXX FIXME -- The rules of the BASIC loader run also when their
# prerequisites are older than the target! This causes the main disk
# image is rebuilt also when the sources have not changed.

# ----------------------------------------------
# G+DOS loader

tmp/loader.gplusdos.bas: \
	tmp/kernel.symbols.gplusdos.z80s \
	src/loader/gplusdos.bas
	gforth make/patch_the_loader.fs $^ $@

tmp/loader.gplusdos.bas.tap: tmp/loader.gplusdos.bas
	zmakebas -n Autoload -a 1 -o $@ $<

# ----------------------------------------------
# +3DOS loader

tmp/loader.plus3dos.bas: \
	tmp/kernel.symbols.plus3dos.z80s \
	src/loader/plus3dos.bas
	gforth make/patch_the_loader.fs $^ $@

tmp/loader.plus3dos.bas.tap: tmp/loader.plus3dos.bas
	zmakebas -n DISK -a 1 -o $@ $<

# ----------------------------------------------
# TR-DOS loader

tmp/loader.trdos.bas: \
	tmp/kernel.symbols.trdos.z80s \
	src/loader/trdos.bas
	gforth make/patch_the_loader.fs $^ $@

tmp/loader.trdos.bas.tap: tmp/loader.trdos.bas
	zmakebas -n boot -a 1 -o $@ $<

# ==============================================================
# Font drivers

# These drivers are not part of the Solo Forth library yet.
# Meanwhile, their binary files are included in the first disk.

# Note: an intermediate file called "pr64.bin" is used, in
# order to force that filename in the TAP header and therefore
# in the disk image.  This file must be in the current
# directory, because the path specified in the command will be
# part of the filename in the TAP header.

# XXX WARNING -- 2016-03-19. bin2code returns error 97 when one
# of the filenames has a path, but it creates the tap file as
# usual.  A hyphen at the beginning of the recipe line forces
# `make` to ignore the error.

tmp/pr64.tap: src/modules/pr64.z80s
	pasmo --tap $< pr64.bin ; \
	mv pr64.bin $@

tmp/pr42.tap: bin/modules/pr42.bin
	cd bin/modules/ ; \
	bin2code pr42.bin pr42.tap 63610 ; \
	cd - ; \
	mv bin/modules/pr42.tap tmp/pr42.tap

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
		tmp/pr64.tap \
		tmp/pr42.tap \
		bin/fonts/ea5a.f42.tap \
		tmp/fzx_fonts.tap
	mkmgt $@ bin/sys/gplusdos-sys-2a.tap $^

# ----------------------------------------------
# +3DOS main disk

tmp/disk0.plus3dos.tap: \
		tmp/loader.plus3dos.bas.tap \
		tmp/kernel.plus3dos.bin.tap \
		tmp/pr64.tap \
		tmp/pr42.tap \
		bin/fonts/ea5a.f42.tap \
		tmp/fzx_fonts.tap
	cat $^ > $@

disks/plus3dos/disk0.720.dsk: tmp/disk0.plus3dos.tap
	tap2dsk -720 -label SoloForth $< $@

disks/plus3dos/disk0.180.dsk: tmp/disk0.plus3dos.tap
	tap2dsk -180 -label SoloForth $< $@

# ----------------------------------------------
# TR-DOS main disk

tmp/disk0.trdos.tap: \
		tmp/loader.trdos.bas.tap \
		tmp/kernel.trdos.bin.tap \
		tmp/pr64.tap \
		tmp/pr42.tap \
		bin/fonts/ea5a.f42.tap \
		tmp/fzx_fonts.tap
	cat $^ > $@

disks/trdos/disk0.trd: tmp/disk0.trdos.tap
	cd tmp && ln -sf $(notdir $<) TRDOS-D0.TAP && cd -
	rm -f $@
	ln -f make/emptytrd.exe make/writetrd.exe tmp/
	cd tmp && \
	echo "EMPTYTRD.EXE SoloFth0.TRD" > mktrd.bat && \
	echo "WRITETRD.EXE SoloFth0.TRD TRDOS-D0.TAP" >> mktrd.bat && \
	dosbox -exit mktrd.bat && \
	cd -
	mv tmp/SOLOFTH0.TRD $@

# ==============================================================
# Library disks

# The library disks contain the source blocks of the library.

lib_files = $(sort $(wildcard src/lib/*.fsb))
dos_lib_files = $(sort $(wildcard src/lib/dos.*.fsb))
game_lib_files = $(sort $(wildcard src/lib/game.*.fsb))
meta_lib_files = $(sort $(wildcard src/lib/meta.*.fsb))
meta_benchmark_misc_lib_files = src/lib/meta.benchmark.misc.fsb
meta_benchmark_rng_lib_files = src/lib/meta.benchmark.rng.fsb
meta_benchmark_flow_lib_files = src/lib/meta.benchmark.flow.fsb
meta_test_lib_files = $(sort $(wildcard src/lib/meta.test*.fsb))
core_lib_files = \
	$(filter-out $(game_lib_files) $(meta_lib_files), \
			$(lib_files))
no_dos_core_lib_files = \
	$(filter-out $(dos_lib_files), $(core_lib_files))

tmp/library_without_dos.fsb: $(no_dos_core_lib_files)
	cat $^ > $@

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
	mv $(basename $<).mgt $@

tmp/library_for_gplusdos+games.fsb: $(gplusdos_core_lib_files) $(game_lib_files)
	cat $(gplusdos_core_lib_files) $(game_lib_files) > $@

disks/gplusdos/disk2_lib+games.mgt: tmp/library_for_gplusdos+games.fsb
	fsb2-mgt $< ;\
	mv $(basename $<).mgt $@

tmp/library_for_gplusdos+misc_benchmarks.fsb: $(gplusdos_core_lib_files) $(meta_benchmark_misc_lib_files)
	cat $^ > $@

disks/gplusdos/disk3_lib+misc_benchmarks.mgt: tmp/library_for_gplusdos+misc_benchmarks.fsb
	fsb2-mgt $< ;\
	mv $(basename $<).mgt $@

tmp/library_for_gplusdos+rng_benchmarks.fsb: $(gplusdos_core_lib_files) $(meta_benchmark_rng_lib_files)
	cat $^ > $@

disks/gplusdos/disk4_lib+rng_benchmarks.mgt: tmp/library_for_gplusdos+rng_benchmarks.fsb
	fsb2-mgt $< ;\
	mv $(basename $<).mgt $@

tmp/library_for_gplusdos+flow_benchmarks.fsb: $(gplusdos_core_lib_files) $(meta_benchmark_flow_lib_files)
	cat $^ > $@

disks/gplusdos/disk5_lib+flow_benchmarks.mgt: tmp/library_for_gplusdos+flow_benchmarks.fsb
	fsb2-mgt $< ;\
	mv $(basename $<).mgt $@

tmp/library_for_gplusdos+tests.fsb: $(gplusdos_core_lib_files) $(meta_test_lib_files)
	cat $^ > $@

disks/gplusdos/disk6_lib+tests.mgt: tmp/library_for_gplusdos+tests.fsb
	fsb2-mgt $< ;\
	mv $(basename $<).mgt $@

disks/gplusdos/disk9_lib_without_dos.mgt: tmp/library_without_dos.fsb
	fsb2-mgt $< ;\
	mv $(basename $<).mgt $@

# XXX TODO -- convert games to FBA
# tmp/nuclear_invaders.fba: src/nuclear_invaders.fs
# 	./make/fs2fba.sh $< ;\
# 	mv $(basename $<).fba $@

# ----------------------------------------------
# +3DOS library disks

# XXX UNDER DEVELOPMENT

plus3dos_core_lib_files = $(filter-out %trdos.fsb %gplusdos.fsb, $(core_lib_files))

tmp/library_for_plus3dos.fsb: $(plus3dos_core_lib_files)
	cat $(gplusdos_core_lib_files) > $@

disks/plus3dos/disk1_lib.dsk: tmp/library_for_plus3dos.fsb
	fsb2-dsk tmp/library_for_plus3dos.fsb ;\
	mv $(basename $<).dsk $@

tmp/library_for_plus3dos+games.fsb: $(plus3dos_core_lib_files) $(game_lib_files)
	cat $(plus3dos_core_lib_files) $(game_lib_files) > $@

disks/plus3dos/disk2_lib+games.dsk: tmp/library_for_plus3dos+games.fsb
	fsb2-dsk $< ;\
	mv $(basename $<).dsk $@

tmp/library_for_plus3dos+misc_benchmarks.fsb: $(plus3dos_core_lib_files) $(meta_benchmark_misc_lib_files)
	cat $^ > $@

disks/plus3dos/disk3_lib+misc_benchmarks.dsk: tmp/library_for_plus3dos+misc_benchmarks.fsb
	fsb2-dsk $< ;\
	mv $(basename $<).dsk $@

tmp/library_for_plus3dos+rng_benchmarks.fsb: $(plus3dos_core_lib_files) $(meta_benchmark_rng_lib_files)
	cat $^ > $@

disks/plus3dos/disk4_lib+rng_benchmarks.dsk: tmp/library_for_plus3dos+rng_benchmarks.fsb
	fsb2-dsk $< ;\
	mv $(basename $<).dsk $@

tmp/library_for_plus3dos+flow_benchmarks.fsb: $(plus3dos_core_lib_files) $(meta_benchmark_flow_lib_files)
	cat $^ > $@

disks/plus3dos/disk5_lib+flow_benchmarks.dsk: tmp/library_for_plus3dos+flow_benchmarks.fsb
	fsb2-dsk $< ;\
	mv $(basename $<).dsk $@

tmp/library_for_plus3dos+tests.fsb: $(plus3dos_core_lib_files) $(meta_test_lib_files)
	cat $^ > $@

disks/plus3dos/disk6_lib+tests.dsk: tmp/library_for_plus3dos+tests.fsb
	fsb2-dsk $< ;\
	mv $(basename $<).dsk $@

disks/plus3dos/disk9_lib_without_dos.dsk: tmp/library_without_dos.fsb
	fsb2-dsk $< ;\
	mv $(basename $<).dsk $@

# ----------------------------------------------
# TR-DOS library disks

trdos_core_lib_files = \
	$(filter-out %gplusdos.fsb %plus3dos.fsb %idedos.fsb, $(core_lib_files))

tmp/library_for_trdos.fsb: $(trdos_core_lib_files)
	cat $(trdos_core_lib_files) > $@

disks/trdos/disk1_lib.trd: tmp/library_for_trdos.fsb 
	fsb2-trd $< SoloFh1 ; \
	mv $(basename $<).trd $@

tmp/library_for_trdos+games.fsb: \
	$(trdos_core_lib_files) $(game_lib_files)
	cat $(trdos_core_lib_files) $(game_lib_files) > $@

disks/trdos/disk2_lib+games.trd: tmp/library_for_trdos+games.fsb
	fsb2-trd $< SoloFth2 ; \
	mv $(basename $<).trd $@

tmp/library_for_trdos+misc_benchmarks.fsb: $(trdos_core_lib_files) $(meta_benchmark_misc_lib_files)
	cat $^ > $@

disks/trdos/disk3_lib+misc_benchmarks.trd: tmp/library_for_trdos+misc_benchmarks.fsb
	fsb2-trd $< SoloFth3 ;\
	mv $(basename $<).trd $@

tmp/library_for_trdos+rng_benchmarks.fsb: $(trdos_core_lib_files) $(meta_benchmark_rng_lib_files)
	cat $^ > $@

disks/trdos/disk4_lib+rng_benchmarks.trd: tmp/library_for_trdos+rng_benchmarks.fsb
	fsb2-trd $< SoloFth4 ;\
	mv $(basename $<).trd $@

tmp/library_for_trdos+flow_benchmarks.fsb: $(trdos_core_lib_files) $(meta_benchmark_flow_lib_files)
	cat $^ > $@

disks/trdos/disk5_lib+flow_benchmarks.trd: tmp/library_for_trdos+flow_benchmarks.fsb
	fsb2-trd $< SoloFth5 ;\
	mv $(basename $<).trd $@

tmp/library_for_trdos+tests.fsb: $(trdos_core_lib_files) $(meta_test_lib_files)
	cat $^ > $@

disks/trdos/disk6_lib+tests.trd: tmp/library_for_trdos+tests.fsb
	fsb2-trd $< SoloFth6 ; \
	mv $(basename $<).trd $@

disks/trdos/disk9_lib_without_dos.trd: tmp/library_without_dos.fsb
	fsb2-trd $< SoloFth9 ; \
	mv $(basename $<).trd $@

# ==============================================================
# Background images

# Starting from version 0.12.0, Solo Forth shows a background image
# every time it boots. It's an illustration made in the 19th century
# by Gustave Doré for the novel _El ingenioso hidalgo don Quijote de
# La Mancha_, by Miguel de Cervantes (1605, 1615).

# First, create a link to the Netpbm image selected for the current
# version of Solo Forth:

backgrounds/current.pbm: src/version.z80s
	version=$(shell make/versionfile2string.fs $<) ; \
	cd backgrounds ; \
	ln -sf v$${version}.pbm $(notdir $@) ; \
	cd ..

# Second, convert it to a SCR format file (which will be included in
# the assembled binary of the system):

backgrounds/current.scr: backgrounds/current.pbm
	make/pbm2scr.fs $<

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
# 2016-08-11: Improve copying the FZX fonts to the disks.
#
# 2016-08-11: Rename the 42 cpl driver and font to make them fit in
# TR-DOS. Rename the 64 cpl driver for the same reason.
#
# 2016-08-11: Make also a 180 KiB disk 0 for +3DOS.
#
# 2016-10-22: Split the benchmarks disk into 3 disks, else it didn't
# fit in a TR-DOS disk image.
#
# 2016-11-16: Modify the cleaning of <tmp/>, to preserve <.gitignore>.
#
# 2016-11-20: Add background images.
#
# 2016-11-21: Adapt: tools used by make have been moved from <tools/>
# to <make/>.
#
# 2016-11-26: Fix lists of benchmark library files.
