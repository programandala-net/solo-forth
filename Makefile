# Makefile

# This file is part of Solo Forth
# http://programandala.net/en.program.solo_forth.html

# Last modified: 202010031656.
# See change log at the end of the file.

# ==============================================================
# Author {{{1

# Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018, 2020.

# ==============================================================
# License {{{1

# You may do whatever you want with this work, so long as you
# retain every copyright, credit and authorship notice, and this
# license.  There is no warranty.

# ==============================================================
# Requirements {{{1

# Asciidoctor (by Dan Allen, Sarah White et al.)
#   http://asciidoctor.org

# Asciidoctor EPUB3 (by Dan Allen and Sarah White)
#   (>v0.5.0.alpha.15)
#   http://github.com/asciidoctor/asciidoctor-epub3

# Asciidoctor PDF (by Dan Allen and Sarah White)
#   http://github.com/asciidoctor/asciidoctor-pdf

# bin2code (by Metalbrain)
#   http://metalbrain.speccy.org/link-eng.htm

# cat (by Torbjorn Granlund and Richard M. Stallman)
#   Part of GNU Coreutils
#   http://gnu.org/software/coreutils

# dbtoepub
#   http://docbook.sourceforge.net/release/xsl/current/epub/README

# dd
#   Part of GNU Coreutils
#   http://gnu.org/software/coreutils

# DOSBox (by The DOSBox Team)
#   http://www.dosbox.com

# Forth Foundation Library (by Dick van Oudheusden)
#   http://irdvo.github.io/ffl/

# fsb2 (by Marcos Cruz)
#   http://programandala.net/en.program.fsb2.html

# Gforth (by Anton Erlt, Bernd Paysan et al.)
#   http://gnu.org/software/gforth

# Glosara (by Marcos Cruz)
#   http://programandala.net/en.program.glosara.html

# head (by David MacKenzie and Jim Meyering)
#   Part of GNU Coreutils
#   http://gnu.org/software/coreutils

# mkmgt (by Marcos Cruz)
#   http://programandala.net/en.program.mkmgt.html

# Pandoc (by John MaFarlane)
#   http://pandoc.org

# sort (by Mike Haertel and Paul Eggert)
#   Part of GNU Coreutils
#   http://gnu.org/software/coreutils

# tap2dsk (by John Elliott)
#    Part of taptools
#    http://www.seasip.info/ZX/unix.html

# zmakebas (by Russell Marks)
#   Usually included in Linux distros. Also see:
#   http://sourceforge.net/p/emuscriptoria/code/HEAD/tree/desprot/ZMakeBas.c
#   https://github.com/catseye/zmakebas
#   http://zmakebas.sourcearchive.com/documentation/1.2-1/zmakebas_8c-source.html

# zx7 (by Einar Saukas)
#   http://www.worldofspectrum.org/infoseekid.cgi?id=0027996

# ==============================================================
# Config {{{1

VPATH = ./

MAKEFLAGS = --no-print-directory

#.ONESHELL:

# ==============================================================
# Metadata {{{1

full_version=$(shell gforth -e 's" ../src/version.z80s" 3' make/version_number.fs)
release=$(shell gforth -e 's" ../src/version.z80s" 2' make/version_number.fs)

# ==============================================================
# Interface {{{1

.PHONY: all
all: gplusdos trdos plus3dos

.PHONY: gplusdos
gplusdos: gplusdosdisks

.PHONY: gplusdosdisks
gplusdosdisks: \
	disks/gplusdos/disk_0_boot.mgt \
	disks/gplusdos/disk_1_library.mgt \
	disks/gplusdos/disk_2_programs.mgt \
	disks/gplusdos/disk_3_workbench.mgt

.PHONY: plus3dos
plus3dos: plus3dosdisks

.PHONY: plus3dosdisks
plus3dosdisks: \
	disks/plus3dos/disk_0_boot.dsk \
	disks/plus3dos/disk_1_library.dsk \
	disks/plus3dos/disk_2_programs.dsk \
	disks/plus3dos/disk_3_workbench.dsk

.PHONY: trdos
trdos: trdosdisks

.PHONY: trdosdisks
trdosdisks: \
	trdos128 \
	pentagon \
	scorpion

.PHONY: trdosblockdisks
trdosblockdisks: \
	disks/trdos/disk_1a_library.trd \
	disks/trdos/disk_1b_library.trd \
	disks/trdos/disk_2_programs.trd \
	disks/trdos/disk_3_workbench.trd

.PHONY: t128
t128: trdos128

.PHONY: trdos128
trdos128: \
	disks/trdos/disk_0_boot.128.trd \
	trdosblockdisks

.PHONY: pentagon
pentagon: \
	disks/trdos/disk_0_boot.pentagon_512.trd \
	disks/trdos/disk_0_boot.pentagon_1024.trd \
	trdosblockdisks

.PHONY: scorpion
scorpion: \
	disks/trdos/disk_0_boot.scorpion_zs_256.trd \
	trdosblockdisks

.PHONY: disk_9
disk_9: \
	disks/gplusdos/disk_9_library_without_dos.mgt \
	disks/plu3sdos/disk_9_library_without_dos.dsk \
	disks/trdos/disk_9_library_without_dos.trd

# XXX REMARK -- disk_9 is a special disk image used for
# debugging. It contains the core library except the DOS
# modules. This means disk_9 contains exactly the same blocks in
# all DOS implementations. This is useful to check if disk
# access works fine when a new DOS is implemented, comparing
# the output of `blks` (a debug tool, temporarily included in
# the kernel).

.PHONY: clean
clean: cleantmp cleandisks cleandoc

.PHONY: cleandisks
cleandisks: cleangplusdosdisks cleanplus3dosdisks cleantrdosdisks

.PHONY: cleangplusdosdisks
cleangplusdosdisks:
	rm -f disks/gplusdos/*.mgt

.PHONY: cleanplus3dosdisks
cleanplus3dosdisks:
	rm -f disks/plus3dos/*.dsk

.PHONY: cleantrdosdisks
cleantrdosdisks:
	rm -f disks/trdos/*.trd

.PHONY: cleantmp
cleantmp:
	-rm -f tmp/[a-zA-Z0-9_]*
# Note: The file <tmp/.gitignore> must be preserved;
#       that's why the wildcard is used.

.PHONY: cleandoc
cleandoc:
	-rm -f doc/* tmp/doc.*

.PHONY: doc
doc: gplusdosdoc plus3dosdoc trdosdoc

.PHONY: gplusdosdoc
gplusdosdoc: gplusdosepub gplusdoshtml gplusdospdf

.PHONY: plus3dosdoc
plus3dosdoc: plus3dosepub plus3doshtml plus3dospdf

.PHONY: trdosdoc
trdosdoc: trdosepub trdoshtml trdospdf

.PHONY: dbk
dbk: gplusdosdbk plus3dosdbk trdosdbk

.PHONY: gplusdosdbk
gplusdosdbk: \
	doc/gplusdos_solo_forth_manual.dbk

.PHONY: plus3dosdbk
plus3dosdbk: \
	doc/plus3dos_solo_forth_manual.dbk

.PHONY: trdosdbk
trdosdbk: \
	doc/trdos_solo_forth_manual.dbk

.PHONY: epub
epub: gplusdosepub plus3dosepub trdosepub

.PHONY: epuba
epuba: gplusdosepuba plus3dosepuba trdosepuba

.PHONY: gplusdosepub
gplusdosepub: gplusdosepuba gplusdosepubd

.PHONY: gplusdosepuba
gplusdosepuba: \
	doc/gplusdos_solo_forth_manual.epub

.PHONY: gplusdosepubd
gplusdosepubd: \
	doc/gplusdos_solo_forth_manual.dbk.dbtoepub.epub

.PHONY: gplusdosepubp
gplusdosepubp: \
	doc/gplusdos_solo_forth_manual.dbk.pandoc.epub

.PHONY: plus3dosepub
plus3dosepub: plus3dosepuba plus3dosepubd

.PHONY: plus3dosepuba
plus3dosepuba: \
	doc/plus3dos_solo_forth_manual.epub

.PHONY: plus3dosepubd
plus3dosepubd: \
	doc/plus3dos_solo_forth_manual.dbk.dbtoepub.epub

.PHONY: plus3dosepubp
plus3dosepubp: \
	doc/plus3dos_solo_forth_manual.dbk.pandoc.epub

.PHONY: trdosepub
trdosepub: trdosepuba trdosepubd

.PHONY: trdosepuba
trdosepuba: \
	doc/trdos_solo_forth_manual.epub

.PHONY: trdosepubd
trdosepubd: \
	doc/trdos_solo_forth_manual.dbk.dbtoepub.epub

.PHONY: trdosepubp
trdosepubp: \
	doc/trdos_solo_forth_manual.dbk.pandoc.epub

.PHONY: html
html: gplusdoshtml plus3doshtml trdoshtml

.PHONY: gplusdoshtml
gplusdoshtml: gplusdoshtmla

.PHONY: gplusdoshtmla
gplusdoshtmla: \
		doc/gplusdos_solo_forth_manual.html.gz

.PHONY: gplusdoshtmlp
gplusdoshtmlp: \
		doc/gplusdos_solo_forth_manual.dbk.pandoc.html.gz

.PHONY: plus3doshtml
plus3doshtml: plus3doshtmla

.PHONY: plus3doshtmla
plus3doshtmla: \
		doc/plus3dos_solo_forth_manual.html.gz

.PHONY: plus3doshtmlp
plus3doshtmlp: \
		doc/plus3dos_solo_forth_manual.dbk.pandoc.html.gz

.PHONY: trdoshtml
trdoshtml: trdoshtmla

.PHONY: trdoshtmla
trdoshtmla: \
		doc/trdos_solo_forth_manual.html.gz

.PHONY: trdoshtmlp
trdoshtmlp: \
		doc/trdos_solo_forth_manual.dbk.pandoc.html.gz

.PHONY: odt
odt: gplusdosodt plus3dosodt trdosodt

.PHONY: gplusdosodt
gplusdosodt: \
		doc/gplusdos_solo_forth_manual.dbk.pandoc.odt

.PHONY: plus3dosodt
plus3dosodt: \
		doc/plus3dos_solo_forth_manual.dbk.pandoc.odt

.PHONY: trdosodt
trdosodt: \
		doc/trdos_solo_forth_manual.dbk.pandoc.odt

.PHONY: pdf
pdf: gplusdospdf plus3dospdf trdospdf

.PHONY: gplusdospdf
gplusdospdf: \
		doc/gplusdos_solo_forth_manual.pdf.gz

.PHONY: plus3dospdf
plus3dospdf: \
		doc/plus3dos_solo_forth_manual.pdf.gz

.PHONY: trdospdf
trdospdf: \
		doc/trdos_solo_forth_manual.pdf.gz

# ==============================================================
# Debug {{{1

# .PHONY: try

# ==============================================================
# Kernel {{{1

include Makefile.pasmo

# ==============================================================
# Loader {{{1

# The BASIC loader of the system is coded in plain text. The addresses
# that depend on the kernel (its load address and entry points) are
# represented in the source by labels. A Forth program replaces the
# labels with the actual values, extracted from the symbols file
# created by the assembler. Then zmakebas converts the patched loader
# into a TAP file, ready to be copied to a disk image.

# ----------------------------------------------
# G+DOS loader {{{2

tmp/loader.gplusdos.bas: \
	tmp/kernel.symbols.gplusdos.z80s \
	src/loader/gplusdos.bas \
	src/kernel.z80s
	gforth make/patch_the_loader.fs $@ $^

tmp/loader.gplusdos.bas.tap: tmp/loader.gplusdos.bas
	zmakebas -n Autoload -a 1 -o $@ $<

# ----------------------------------------------
# +3DOS loader {{{2

tmp/loader.plus3dos.bas: \
	tmp/kernel.symbols.plus3dos.z80s \
	src/loader/plus3dos.bas \
	src/kernel.z80s
	gforth make/patch_the_loader.fs $@ $^

tmp/loader.plus3dos.bas.tap: tmp/loader.plus3dos.bas
	zmakebas -n DISK -a 1 -o $@ $<

# ----------------------------------------------
# TR-DOS loader {{{2

tmp/loader.trdos.bas: \
	tmp/kernel.symbols.trdos.z80s \
	src/loader/trdos.bas \
	src/kernel.z80s
	gforth make/patch_the_loader.fs $@ $^

tmp/loader.trdos.bas.tap: \
	tmp/loader.trdos.bas
	zmakebas -n boot -a 1 -o $@ $<

# ==============================================================
# Addons {{{1

# These addons (font drivers) are not part of the Solo Forth
# library yet.  Meanwhile, their binary files are included in
# the boot disk.

# XXX WARNING -- 2016-03-19. bin2code returns error 97 when one
# of the filenames has a path, but it creates the tap file as
# usual.  A hyphen at the beginning of the target forces
# `make` to ignore the error.

tmp/pr42.tap: bin/addons/pr42.bin
	cd bin/addons/ ; \
	bin2code pr42.bin pr42.tap 63610 ; \
	cd - ; \
	mv bin/addons/pr42.tap tmp/pr42.tap

# ==============================================================
# Fonts {{{1

# Note: The DSK disk image needs the fzx files to be packed
# into TAP first.

fzx_fonts = $(wildcard bin/fonts/*.fzx)

tmp/fzx_fonts.tap : $(fzx_fonts)
	cd bin/fonts ; \
	for file in $(notdir $(fzx_fonts)); do \
		bin2code $$file $$file.tap; \
	done; \
	cd -
	cat $(addsuffix .tap,$(fzx_fonts)) > $@ ; \
	rm -f $(addsuffix .tap, $(fzx_fonts))

f64_fonts = $(wildcard bin/fonts/*.f64)

tmp/f64_fonts.tap : $(f64_fonts)
	cd bin/fonts ; \
	for file in $(notdir $(f64_fonts)); do \
		bin2code $$file $$file.tap; \
	done; \
	cd -
	cat $(addsuffix .tap,$(f64_fonts)) > $@ ; \
	rm -f $(addsuffix .tap, $(f64_fonts))

# ==============================================================
# Compressed test screen {{{1

# A ZX Spectrum screen compressed with ZX7 is included in the
# boot disk, in order to try the ZX7 decompressor.

tmp/img.tap: bin/test/img.zx7
	cd bin/test/ ; \
	bin2code img.zx7 img.tap 16384 ; \
	cd - ; \
	mv bin/test/img.tap tmp/img.tap

# ==============================================================
# Boot disk {{{1

# ----------------------------------------------
# G+DOS boot disk {{{2

disks/gplusdos/disk_0_boot.mgt: \
		tmp/loader.gplusdos.bas.tap \
		tmp/kernel.gplusdos.bin.tap \
		tmp/pr42.tap \
		bin/fonts/ea5a.f42.tap \
		tmp/f64_fonts.tap \
		tmp/fzx_fonts.tap \
		tmp/img.tap
	mkmgt $@ bin/dos/gplusdos-sys-2a.tap $^

# ----------------------------------------------
# +3DOS boot disk {{{2

tmp/disk_0_boot.plus3dos.tap: \
		tmp/loader.plus3dos.bas.tap \
		tmp/kernel.plus3dos.bin.tap \
		tmp/pr42.tap \
		bin/fonts/ea5a.f42.tap \
		tmp/f64_fonts.tap \
		tmp/fzx_fonts.tap \
		tmp/img.tap
	cat $^ > $@

disks/plus3dos/disk_0_boot.dsk: tmp/disk_0_boot.plus3dos.tap
	tap2dsk -720 -label SoloForth $< $@

# ----------------------------------------------
# TR-DOS boot disk {{{2

tmp/disk_0_boot.trdos.tap: \
		tmp/loader.trdos.bas.tap \
		tmp/kernel.trdos.bin.tap \
		tmp/pr42.tap \
		bin/fonts/ea5a.f42.tap \
		tmp/f64_fonts.tap \
		tmp/fzx_fonts.tap \
		tmp/img.tap
	cat $^ > $@

disks/trdos/disk_0_boot.128.trd: tmp/disk_0_boot.trdos.tap
	cd tmp && ln -sf $(notdir $<) TRDOS-D0.TAP && cd -
	rm -f $@
	ln -f make/emptytrd.exe make/writetrd.exe tmp/
	cd tmp && \
	echo "EMPTYTRD.EXE SoloFth0.TRD" > mktrd.bat && \
	echo "WRITETRD.EXE SoloFth0.TRD TRDOS-D0.TAP" >> mktrd.bat && \
	dosbox -exit mktrd.bat && \
	cd -
	mv tmp/SOLOFTH0.TRD $@

# ----------------------------------------------
# TR-DOS boot disk for Scorpion ZS 256 {{{2

tmp/disk_0_boot.trdos.scorpion_zs_256.tap: \
		tmp/loader.trdos.bas.tap \
		tmp/kernel.trdos.scorpion_zs_256.bin.tap \
		tmp/pr42.tap \
		bin/fonts/ea5a.f42.tap \
		tmp/f64_fonts.tap \
		tmp/fzx_fonts.tap
	cat $^ > $@

disks/trdos/disk_0_boot.scorpion_zs_256.trd: tmp/disk_0_boot.trdos.scorpion_zs_256.tap
	cd tmp && ln -sf $(notdir $<) TRDOS-D0.TAP && cd -
	rm -f $@
	ln -f make/emptytrd.exe make/writetrd.exe tmp/
	cd tmp && \
	echo "EMPTYTRD.EXE SoloFth0.TRD" > mktrd.bat && \
	echo "WRITETRD.EXE SoloFth0.TRD TRDOS-D0.TAP" >> mktrd.bat && \
	dosbox -exit mktrd.bat && \
	cd -
	mv tmp/SOLOFTH0.TRD $@

# ----------------------------------------------
# TR-DOS boot disk for Pentagon 512 {{{2

tmp/disk_0_boot.trdos.pentagon_512.tap: \
		tmp/loader.trdos.bas.tap \
		tmp/kernel.trdos.pentagon_512.bin.tap \
		tmp/pr42.tap \
		bin/fonts/ea5a.f42.tap \
		tmp/f64_fonts.tap \
		tmp/fzx_fonts.tap
	cat $^ > $@

disks/trdos/disk_0_boot.pentagon_512.trd: tmp/disk_0_boot.trdos.pentagon_512.tap
	cd tmp && ln -sf $(notdir $<) TRDOS-D0.TAP && cd -
	rm -f $@
	ln -f make/emptytrd.exe make/writetrd.exe tmp/
	cd tmp && \
	echo "EMPTYTRD.EXE SoloFth0.TRD" > mktrd.bat && \
	echo "WRITETRD.EXE SoloFth0.TRD TRDOS-D0.TAP" >> mktrd.bat && \
	dosbox -exit mktrd.bat && \
	cd -
	mv tmp/SOLOFTH0.TRD $@

# ----------------------------------------------
# TR-DOS boot disk for Pentagon 1024 {{{2

tmp/disk_0_boot.trdos.pentagon_1024.tap: \
		tmp/loader.trdos.bas.tap \
		tmp/kernel.trdos.pentagon_1024.bin.tap \
		tmp/pr42.tap \
		bin/fonts/ea5a.f42.tap \
		tmp/f64_fonts.tap \
		tmp/fzx_fonts.tap
	cat $^ > $@

disks/trdos/disk_0_boot.pentagon_1024.trd: tmp/disk_0_boot.trdos.pentagon_1024.tap
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
# Source file lists {{{1

#not_ready = src/lib/meta.test.forth2012-test-suite.fs
not_ready =

lib_files = $(sort $(wildcard src/lib/*.fs))
dos_lib_files = $(sort $(wildcard src/lib/dos.*.fs))
editor_prog_lib_files = $(sort $(wildcard src/lib/prog.editor.*.fs))
app_prog_lib_files = $(sort $(wildcard src/lib/prog.app.*.fs))
prog_lib_files = $(sort $(wildcard src/lib/prog.*.fs))
exception_codes_lib_files = $(sort $(wildcard src/lib/exception.codes.*.fs))

meta_lib_files = $(filter-out $(not_ready),$(sort $(wildcard src/lib/meta.*.fs)))

# XXX OLD:
meta_benchmark_lib_files = $(filter-out $(not_ready),$(sort $(wildcard src/lib/meta.benchmark.*.fs)))
meta_benchmark_misc_lib_files = src/lib/meta.benchmark.MISC.fs
meta_benchmark_rng_lib_files = src/lib/meta.benchmark.rng.fs
meta_benchmark_flow_lib_files = src/lib/meta.benchmark.flow.fs
meta_test_lib_files = $(filter-out $(not_ready),$(sort $(wildcard src/lib/meta.test*.fs)))

core_lib_files = \
	$(filter-out \
			$(not_ready) $(prog_lib_files) $(meta_lib_files), \
			$(lib_files))

no_dos_core_lib_files = \
	$(filter-out $(dos_lib_files), $(core_lib_files))

gplusdos_core_lib_files = \
	$(filter-out %idedos.fs %plus3dos.fs %trdos.fs, $(core_lib_files))

plus3dos_core_lib_files = \
	$(filter-out %gplusdos.fs %idedos.fs %trdos.fs, $(core_lib_files))

trdos_core_lib_files = \
	$(filter-out %gplusdos.fs %idedos.fs %plus3dos.fs, $(core_lib_files))

gplusdos_exception_codes_lib_files = \
	$(filter-out %idedos.fs %plus3dos.fs %trdos.fs , $(exception_codes_lib_files))

plus3dos_exception_codes_lib_files = \
	$(filter-out %gplusdos.fs %idedos.fs %trdos.fs, $(exception_codes_lib_files))

trdos_exception_codes_lib_files = \
	$(filter-out %gplusdos.fs %idedos.fs %plus3dos.fs, $(exception_codes_lib_files))

# ==============================================================
# Block files {{{1

# XXX UNDER DEVELOPMENT

# Copying the whole library to a disk image, as one single
# file, is not possible with ordinary tools, which are written
# to manipulate only the ordinary filetypes that can be stored
# on a ZX Spectrum tape.
#
# Therefore, this first approach can not work:

%.fb: %.fs
	fsb2 $<

library_block_file=solo.fb

tmp/library.gplusdos.tap: tmp/library.gplusdos.fb
	cd tmp/; \
	ln -f $(notdir $<) $(library_block_file);\
	bin2code $(library_block_file) $(library_block_file).tap 0;\
	mv $(library_block_file).tap $(notdir $@);\
	cd -;

tmp/library.trdos.tap: tmp/library.trdos.fb
	cd tmp/; \
	ln -f $(notdir $<) $(library_block_file);\
	bin2code $(library_block_file) $(library_block_file).tap 0;\
	mv $(library_block_file).tap $(notdir $@);\
	cd -;

tmp/library.plus3dos.tap: tmp/library.plus3dos.fb
	cd tmp/; \
	ln -sf $(notdir $<) $(library_block_file);\
	bin2code $(library_block_file) $(library_block_file).tap 0;\
	mv $(library_block_file).tap $(notdir $@);\
	cd -;

# ==============================================================
# Block disks {{{1

# The block disks contain the source blocks of the library and
# additional code.

tmp/library_without_dos.fs: $(no_dos_core_lib_files)
	cat $^ > $@

tmp/workbench.fs: $(meta_lib_files)
	cat $^ > $@

tmp/programs.fs: $(prog_lib_files)
	cat $^ > $@

# ----------------------------------------------
# G+DOS block disks {{{2

# ------------------------------
# Library disk {{{3

tmp/library.gplusdos.fs: $(gplusdos_core_lib_files)
	cat $(gplusdos_core_lib_files) > $@

disks/gplusdos/disk_1_library.mgt: tmp/library.gplusdos.fs
	fsb2-mgt $< ;\
	mv $(basename $<).mgt $@

# ------------------------------
# Additional disks {{{3

disks/gplusdos/disk_2_programs.mgt: tmp/programs.fs
	fsb2-mgt $< ;\
	mv $(basename $<).mgt $@

disks/gplusdos/disk_3_workbench.mgt: tmp/workbench.fs
	fsb2-mgt $< ;\
	mv $(basename $<).mgt $@

# ----------------------------------------------
# +3DOS block disks {{{2

# ------------------------------
# Library disk {{{3

tmp/library.plus3dos.fs: $(plus3dos_core_lib_files)
	cat $(plus3dos_core_lib_files) > $@

disks/plus3dos/disk_1_library.dsk: tmp/library.plus3dos.fs
	fsb2-dsk tmp/library.plus3dos.fs ;\
	mv $(basename $<).dsk $@

# ------------------------------
# Additional disks {{{3

disks/plus3dos/disk_2_programs.dsk: tmp/programs.fs
	fsb2-dsk $< ;\
	mv $(basename $<).dsk $@

disks/plus3dos/disk_3_workbench.dsk: tmp/workbench.fs
	fsb2-dsk $< ;\
	mv $(basename $<).dsk $@

# ----------------------------------------------
# TR-DOS block disks {{{2

# ------------------------------
# Library disks {{{3

tmp/library.trdos.fs: $(trdos_core_lib_files)
	cat $(trdos_core_lib_files) > $@

tmp/library.trdos.fb: tmp/library.trdos.fs
	fsb2 $<

tmp/library_a.trdos.fb: tmp/library.trdos.fb
	dd if=$< of=$@ bs=1K count=636

tmp/library_b.trdos.fb: tmp/library.trdos.fb
	dd if=$< of=$@ bs=1K count=636 skip=636

tmp/library_a.trdos.fs: tmp/library_a.trdos.fb
	dd if=$< of=$@ bs=1K cbs=64 conv=unblock

tmp/library_b.trdos.fs: tmp/library_b.trdos.fb
	dd if=$< of=$@ bs=1K cbs=64 conv=unblock

disks/trdos/disk_1a_library.trd: tmp/library_a.trdos.fs
	fsb2-trd $< SoloF1a ; \
	mv $(basename $<).trd $@

disks/trdos/disk_1b_library.trd: tmp/library_b.trdos.fs
	fsb2-trd $< SoloF1b ; \
	mv $(basename $<).trd $@

# ------------------------------
# Additional disks {{{3

disks/trdos/disk_2_programs.trd: tmp/programs.fs
	fsb2-trd $< SoloFth2 ; \
	mv $(basename $<).trd $@

disks/trdos/disk_3_workbench.trd: tmp/workbench.fs
	fsb2-trd $< SoloFth3 ; \
	mv $(basename $<).trd $@

# ==============================================================
# Background images {{{1

# Starting from version 0.12.0, Solo Forth shows a background
# image every time it boots.

# First, create a link to the Netpbm image selected for the current
# version of Solo Forth:

backgrounds/current.pbm: src/version.z80s
	version=$(shell gforth -e 's" ../$<" 0' make/version_number.fs) ; \
	cd backgrounds ; \
	cp -f v$${version}.pbm $(notdir $@) ; \
	cd ..

#	ln -f v$${version}.pbm $(notdir $@) ; \

# Second, convert it to a SCR format file (which will be included in
# the assembled binary of the system):

backgrounds/current.scr: backgrounds/current.pbm
	make/pbm2scr.fs $<

# ==============================================================
# Documentation {{{1

# ----------------------------------------------
# Common rules {{{2

%.zip: %
	zip -9 $@ $<

%.gz: %
	gzip -9 --force --keep $<

%.html: %.adoc
	asciidoctor --out-file=$@ $<

%.glossary.adoc: %.files.txt
	glosara --level=4 --sections --input=$< > $@

tmp/doc.%.linked.adoc: src/doc/%.adoc
	glosara --annex $< > $@

tmp/doc.README.linked.adoc: README.adoc
	glosara --annex $< > $@

%doc.gplusdos.manual.dbk: %doc.gplusdos.manual.adoc
	asciidoctor \
		--backend=docbook \
		--attribute=gplusdos \
		--attribute=dosname=G+DOS \
		--attribute=version=$(full_version) \
		--out-file=$@ $<

%doc.plus3dos.manual.dbk: %doc.plus3dos.manual.adoc
	asciidoctor \
		--backend=docbook \
		--attribute=plus3dos \
		--attribute=dosname=+3DOS \
		--attribute=version=$(full_version) \
		--out-file=$@ $<

%doc.trdos.manual.dbk: %doc.trdos.manual.adoc
	asciidoctor \
		--backend=docbook \
		--attribute=trdos \
		--attribute=dosname=TR-DOS \
		--attribute=version=$(full_version) \
		--out-file=$@ $<

%.dbk.dbtoepub.epub: %.dbk
	dbtoepub -o $@ $<

# XXX REMARK -- Pandoc is not used anymore to create an EPUB, because the
# internal links don't work:

%.dbk.pandoc.epub: %.dbk
	pandoc \
		--from docbook \
		--to epub \
		--epub-chapter-level=3 \
		--output=$@ $<

# XXX REMARK -- Pandoc is not used anymore to create an HTML, because the
# glossary links don't work:

%.dbk.pandoc.html: %.dbk
	pandoc \
		--from docbook \
		--to html \
		--output=$@ $<

%.dbk.pandoc.odt: %.dbk
	pandoc \
		--from docbook \
		--to odt \
		--output=$@ $<

# XXX REMARK -- 2020-04-05: This PDF is not build by default anymore, because
# the links don't work:

%.html.pandoc.pdf: %.html
	pandoc \
		--from html \
		--to pdf \
		--pdf-engine=wkhtmltopdf \
		--output=$@ $<

# ----------------------------------------------
# Documentation for G+DOS {{{2

doc/gplusdos_solo_forth_manual.epub: \
	tmp/doc.gplusdos.manual.adoc \
	README.adoc
	asciidoctor-epub3 \
		--trace \
		--attribute=gplusdos \
		--attribute=dosname=G+DOS \
		--attribute=epub-chapter-level=2 \
		--attribute=version=$(full_version) \
		--out-file=$@ $< \
		2> tmp/doc.gplusdos.unknown_anchors.log

doc/gplusdos_solo_forth_manual.pdf: \
	tmp/doc.gplusdos.manual.adoc \
	README.adoc
	asciidoctor-pdf \
		--trace \
		--attribute=gplusdos \
		--attribute=dosname=G+DOS \
		--attribute=version=$(full_version) \
		--out-file=$@ $<

doc/gplusdos_solo_forth_manual.html: \
	tmp/doc.gplusdos.manual.adoc \
	README.adoc
	asciidoctor \
		--attribute=gplusdos \
		--attribute=dosname=G+DOS \
		--attribute=version=$(full_version) \
		--out-file=$@ $<

tmp/doc.gplusdos.files.txt: \
	src/kernel.z80s \
	src/kernel.gplusdos.z80s \
	$(app_prog_lib_files) \
	$(editor_prog_lib_files) \
	$(meta_test_lib_files) \
	$(gplusdos_core_lib_files)
	ls -1 $^ > $@

tmp/doc.gplusdos.exception_codes.adoc: $(gplusdos_exception_codes_lib_files)
	grep \
		--no-filename "^#-[0-9]\+\s[\]\s[[:print:]]" $^ | \
	sed \
		-e "s/^/| /" \
		-e "s/[\]/|/" \
		-e "1s/^/[%autowidth]\n.Exception code assignments\n\|===\n|Exception code|Meaning\n\n/" \
	> $@ && \
	echo "|===" >> $@

#	XXX FIXME -- 2018-07-20:
#	sed -e "s/^/| /" -e "s/[\]/|/" -e "1s/^/\|===OPEN\n/" -e "/$$/s/$$/\n|===CLOSE/" \
# XXX REMARK -- `echo` was used instead.

# Preserve the links in the DocBook source by removing the
# enclosing <literal> tags:

doc/gplusdos_solo_forth_manual.dbk: tmp/doc.gplusdos.manual.dbk
	sed \
		-e "s/<literal><link/<link/g" \
		-e "s/<\/link><\/literal>/<\/link>/g" $< > $@

tmp/doc.gplusdos.manual.adoc: \
	tmp/doc.manual_skeleton.linked.adoc \
	tmp/doc.stack_notation.linked.adoc \
	tmp/doc.z80_flags_notation.linked.adoc \
	tmp/doc.z80_instructions.linked.adoc \
	src/doc/glossary_heading.adoc \
	tmp/doc.gplusdos.glossary.adoc \
	tmp/doc.gplusdos.exception_codes.adoc \
	tmp/doc.README.linked.adoc \
	src/version.z80s
	cat \
		tmp/doc.manual_skeleton.linked.adoc \
		tmp/doc.stack_notation.linked.adoc \
		tmp/doc.z80_flags_notation.linked.adoc \
		tmp/doc.z80_instructions.linked.adoc \
		src/doc/glossary_heading.adoc \
		tmp/doc.gplusdos.glossary.adoc \
		> $@

# ----------------------------------------------
# Documentation for +3DOS {{{2

doc/plus3dos_solo_forth_manual.epub: \
	tmp/doc.plus3dos.manual.adoc \
	README.adoc
	asciidoctor-epub3 \
		--trace \
		--attribute=plus3dos \
		--attribute=dosname=+3DOS \
		--attribute=epub-chapter-level=2 \
		--attribute=version=$(full_version) \
		--out-file=$@ $< \
		2> tmp/doc.plus3dos.unknown_anchors.log

doc/plus3dos_solo_forth_manual.pdf: \
	tmp/doc.plus3dos.manual.adoc \
	README.adoc
	asciidoctor-pdf \
		--trace \
		--attribute=plus3dos \
		--attribute=dosname=+3DOS \
		--attribute=version=$(full_version) \
		--out-file=$@ $<

doc/plus3dos_solo_forth_manual.html: \
	tmp/doc.plus3dos.manual.adoc \
	README.adoc
	asciidoctor \
		--attribute=plus3dos \
		--attribute=dosname=+3DOS \
		--attribute=version=$(full_version) \
		--out-file=$@ $<

tmp/doc.plus3dos.files.txt: \
	src/kernel.z80s \
	src/kernel.plus3dos.z80s \
	$(app_prog_lib_files) \
	$(editor_prog_lib_files) \
	$(meta_test_lib_files) \
	$(plus3dos_core_lib_files)
	ls -1 $^ > $@

tmp/doc.plus3dos.exception_codes.adoc: $(plus3dos_exception_codes_lib_files)
	grep \
		--no-filename "^#-[0-9]\+\s[\]\s[[:print:]]" $^ | \
	sed \
		-e "s/^/| /" \
		-e "s/[\]/|/" \
		-e "1s/^/[%autowidth]\n.Exception code assignments\n\|===\n|Exception code|Meaning\n\n/" \
	> $@ && \
	echo "|===" >> $@

# Preserve the links in the DocBook source by removing the
# enclosing <literal> tags:

doc/plus3dos_solo_forth_manual.dbk: tmp/doc.plus3dos.manual.dbk
	sed \
		-e "s/<literal><link/<link/g" \
		-e "s/<\/link><\/literal>/<\/link>/g" $< > $@

tmp/doc.plus3dos.manual.adoc: \
	tmp/doc.manual_skeleton.linked.adoc \
	tmp/doc.stack_notation.linked.adoc \
	tmp/doc.z80_flags_notation.linked.adoc \
	tmp/doc.z80_instructions.linked.adoc \
	src/doc/glossary_heading.adoc \
	tmp/doc.plus3dos.glossary.adoc \
	tmp/doc.plus3dos.exception_codes.adoc \
	tmp/doc.README.linked.adoc \
	src/version.z80s
	cat \
		tmp/doc.manual_skeleton.linked.adoc \
		tmp/doc.stack_notation.linked.adoc \
		tmp/doc.z80_flags_notation.linked.adoc \
		tmp/doc.z80_instructions.linked.adoc \
		src/doc/glossary_heading.adoc \
		tmp/doc.plus3dos.glossary.adoc \
		> $@

# ----------------------------------------------
# Documentation for TR-DOS {{{2

doc/trdos_solo_forth_manual.epub: \
	tmp/doc.trdos.manual.adoc \
	README.adoc
	asciidoctor-epub3 \
		--trace \
		--attribute=trdos \
		--attribute=dosname=TRDOS \
		--attribute=epub-chapter-level=2 \
		--attribute=version=$(full_version) \
		--out-file=$@ $< \
		2> tmp/doc.trdos.unknown_anchors.log

doc/trdos_solo_forth_manual.pdf: \
	tmp/doc.trdos.manual.adoc \
	README.adoc
	asciidoctor-pdf \
		--trace \
		--attribute=trdos \
		--attribute=dosname=TR-DOS \
		--attribute=version=$(full_version) \
		--out-file=$@ $<

doc/trdos_solo_forth_manual.html: \
	tmp/doc.trdos.manual.adoc \
	README.adoc
	asciidoctor \
		--attribute=trdos \
		--attribute=dosname=TR-DOS \
		--attribute=version=$(full_version) \
		--out-file=$@ $<

tmp/doc.trdos.files.txt: \
	src/kernel.z80s \
	src/kernel.trdos.z80s \
	$(app_prog_lib_files) \
	$(editor_prog_lib_files) \
	$(meta_test_lib_files) \
	$(trdos_core_lib_files)
	ls -1 $^ > $@

tmp/doc.trdos.exception_codes.adoc: $(trdos_exception_codes_lib_files)
	grep \
		--no-filename "^#-[0-9]\+\s[\]\s[[:print:]]" $^ | \
	sed \
		-e "s/^/| /" \
		-e "s/[\]/|/" \
		-e "1s/^/[%autowidth]\n.Exception code assignments\n\|===\n|Exception code|Meaning\n\n/" \
	> $@ && \
	echo "|===" >> $@

# Preserve the links in the DocBook source by removing the
# enclosing <literal> tags:

doc/trdos_solo_forth_manual.dbk: tmp/doc.trdos.manual.dbk
	sed \
		-e "s/<literal><link/<link/g" \
		-e "s/<\/link><\/literal>/<\/link>/g" $< > $@

tmp/doc.trdos.manual.adoc: \
	tmp/doc.manual_skeleton.linked.adoc \
	tmp/doc.stack_notation.linked.adoc \
	tmp/doc.z80_flags_notation.linked.adoc \
	tmp/doc.z80_instructions.linked.adoc \
	src/doc/glossary_heading.adoc \
	tmp/doc.trdos.glossary.adoc \
	tmp/doc.trdos.exception_codes.adoc \
	tmp/doc.README.linked.adoc \
	src/version.z80s
	cat \
		tmp/doc.manual_skeleton.linked.adoc \
		tmp/doc.stack_notation.linked.adoc \
		tmp/doc.z80_flags_notation.linked.adoc \
		tmp/doc.z80_instructions.linked.adoc \
		src/doc/glossary_heading.adoc \
		tmp/doc.trdos.glossary.adoc \
		> $@

# ==============================================================
# Release archives {{{1

.PHONY: zips
zips: diskzips doczips srczip

.PHONY: diskzips
diskzips: gplusdosdiskszip plus3dosdiskszip trdosdiskszip

.PHONY: doczips
doczips: gplusdosdoczip plus3dosdoczip trdosdoczip

.PHONY: srczip
srczip: tmp/solo_forth_$(release)_src.zip

# ----------------------------------------------
# Source release archive {{{2

tmp/solo_forth_$(release)_src.zip: \
	Makefile src/ bin/ make/ tools/ vim/ tmp/.gitignore
	cd .. ; \
	ln -sfn solo_forth solo_forth_$(release) ; \
	zip -9r \
		solo_forth_$(release)/$@ $(addprefix solo_forth_$(release)/,$^) \
		--exclude *.swp ; \
	rm -f solo_forth_$(release) 

# ----------------------------------------------
# G+DOS release archives {{{2

.PHONY: gplusdoszips
gplusdoszips: gplusdosdiskszip gplusdosdoczip

.PHONY: gplusdosdoczip
gplusdosdoczip: tmp/solo_forth_$(release)_gplusdos_manuals.zip

tmp/solo_forth_$(release)_gplusdos_manuals.zip: \
	doc/gplusdos_solo_forth_manual.dbk.dbtoepub.epub \
	doc/gplusdos_solo_forth_manual.epub \
	doc/gplusdos_solo_forth_manual.html \
	doc/gplusdos_solo_forth_manual.pdf
	cd .. ; \
	ln -sfn solo_forth solo_forth_$(release) ; \
	zip -9r solo_forth_$(release)/$@ $(addprefix solo_forth_$(release)/,$^) ; \
	rm -f solo_forth_$(release) 

.PHONY: gplusdosdiskszip
gplusdosdiskszip: tmp/solo_forth_$(release)_gplusdos_disks.zip

tmp/solo_forth_$(release)_gplusdos_disks.zip: disks/gplusdos/*.mgt
	cd .. ; \
	ln -sfn solo_forth solo_forth_$(release) ; \
	zip -9r solo_forth_$(release)/$@ $(addprefix solo_forth_$(release)/,$^) ; \
	rm -f solo_forth_$(release) 

# ----------------------------------------------
# +3DOS release archives {{{2

.PHONY: plus3doszips
plus3doszips: plus3dosdiskszip plus3dosdoczip

.PHONY: plus3dosdoczip
plus3dosdoczip: tmp/solo_forth_$(release)_plus3dos_manuals.zip

tmp/solo_forth_$(release)_plus3dos_manuals.zip: \
	doc/plus3dos_solo_forth_manual.dbk.dbtoepub.epub \
	doc/plus3dos_solo_forth_manual.epub \
	doc/plus3dos_solo_forth_manual.html \
	doc/plus3dos_solo_forth_manual.pdf
	cd .. ; \
	ln -sfn solo_forth solo_forth_$(release) ; \
	zip -9r solo_forth_$(release)/$@ $(addprefix solo_forth_$(release)/,$^) ; \
	rm -f solo_forth_$(release) 

.PHONY: plus3dosdiskszip
plus3dosdiskszip: tmp/solo_forth_$(release)_plus3dos_disks.zip

tmp/solo_forth_$(release)_plus3dos_disks.zip: disks/plus3dos/*.dsk
	cd .. ; \
	ln -sfn solo_forth solo_forth_$(release) ; \
	zip -9r solo_forth_$(release)/$@ $(addprefix solo_forth_$(release)/,$^) ; \
	rm -f solo_forth_$(release) 

# ----------------------------------------------
# TR-DOS release archives {{{2

.PHONY: trdoszips
trdoszips: trdosdiskszip trdosdoczip

.PHONY: trdosdoczip
trdosdoczip: tmp/solo_forth_$(release)_trdos_manuals.zip

tmp/solo_forth_$(release)_trdos_manuals.zip: \
	doc/trdos_solo_forth_manual.dbk.dbtoepub.epub \
	doc/trdos_solo_forth_manual.epub \
	doc/trdos_solo_forth_manual.html \
	doc/trdos_solo_forth_manual.pdf
	cd .. ; \
	ln -sfn solo_forth solo_forth_$(release) ; \
	zip -9r solo_forth_$(release)/$@ $(addprefix solo_forth_$(release)/,$^) ; \
	rm -f solo_forth_$(release) 

.PHONY: trdosdiskszip
trdosdiskszip: tmp/solo_forth_$(release)_trdos_disks.zip

tmp/solo_forth_$(release)_trdos_disks.zip: disks/trdos/*.trd
	cd .. ; \
	ln -sfn solo_forth solo_forth_$(release) ; \
	zip -9r solo_forth_$(release)/$@ $(addprefix solo_forth_$(release)/,$^) ; \
	rm -f solo_forth_$(release) 

# ==============================================================
# Backup {{{1

.PHONY: backupsrc
backupsrc:
	tar -cJf backups/$$(date +%Y%m%d%H%M)_solo_forth_src.tar.xz \
		Makefile* \
		src/lib/*.fs \
		src/kernel.z80s

.PHONY: backuplib
backuplib:
	tar -cJf backups/$$(date +%Y%m%d%H%M)_solo_forth_library.tar.xz \
		src/lib/*.fs

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
# Makefile variables cheat sheet {{{1

# $@ = the name of the target of the rule
# $< = the name of the first prerequisite
# $? = the names of all the prerequisites that are newer than the target
# $^ = the names of all the prerequisites

# `%` works only at the start of the filter pattern

# ==============================================================
# Change log {{{1

# 2015-06-02: Start.
#
# 2015-06-17: Improved backup recipe.
#
# 2015-06-29: Improvement: The source filenames are
# configurable. This makes it easier to try old versions, for
# debugging.
#
# 2015-07-22: Added the MGT disk images to the backup.
# Sometimes it's useful to test and old version without
# recompiling the old sources.
#
# 2015-08-14: Updated backup recipe.
#
# 2015-08-17: Modified to use GNU binutils instead of Pasmo.
#
# 2015-08-18: Improved. New: a Forth program creates the
# symbols file.
#
# 2015-08-20: Divided in three parts: Makefile, Makefile.pasmo,
# Makefile.binutils.
#
# 2015-10-10: Substituted fsb (written in Vim) with fsb2
# (written in Forth for Gforth). fsb was becoming too show with
# more than 300 source screens, while fsb2 is instantaneous.
#
# 2015-10-15: Updated.
#
# 2015-11-10: First changes to support also the +3DOS version.
#
# 2015-11-11: The DOS error codes are separated from the main
# file of the library.
#
# 2015-11-12: Fixed the load address of the font drivers; they
# were missing because of the recent use of `bin2code`,
# required to build +3DOS disk images.
#
# 2016-02-15: Added Nuclear Invaders to the library, a game
# under development for Solo Forth, in order to try it.
#
# 2016-02-22: Delete also <library.complete.*.fsb> in clean.
# It seems this is required when parts of the library are
# simbolyc links.
#
# 2016-03-19: Updated after the reorganization of files into
# directories.
#
# 2016-03-22: Removed Nuclear Invaders from the library,
# because now it's built on its own directory.
#
# 2016-03-24: New partial backups.
#
# 2016-04-13: Improved: the BASIC loader is patched with the
# current memory addresses, extracted from the Z80 symbols
# file.  Updated the requirements and the license.
#
# 2016-04-16: Fix: the patching of the loader didn't work after
# `make clean`, because a prerequisite was missing.
#
# 2016-04-16: Also the +3DOS loader is patched with the current
# values of the kernel.
#
# 2016-05-02: Make two library disks: the main one, now without
# games; a new one, with games but without the meta
# benchamarks/tests. This is needed, because the library has
# grown too much, and it's near the limit of a G+DOS disk (800
# KiB).
#
# 2016-05-03: Make three library disks: 1) only with the core
# library; 2) the core library plus the meta tools (benchmarks
# and tests for Solo Forth itself); 3) the core library plus
# the sample games.
#
# 2016-08-03: Make first changes to support TR-DOS.
#
# 2016-08-04: Fix TRD library disks with a track 0.
#
# 2016-08-05: Create the disks images in <disks/DOSNAME>. Split
# the meta tools disk into two: tests and benchmarks. Rename
# all disk image files after a shorter and clearer format. Add
# disk9 for debugging.
#
# 2016-08-10: Activate disk0 of +3DOS.
#
# 2016-08-11: Make TRD disk images with the improved version of
# fsb2-trd, which now creates a sector 0, instead of the ad hoc
# solution.
#
# 2016-08-11: Improve copying the FZX fonts to the disks.
#
# 2016-08-11: Rename the 42 cpl driver and font to make them
# fit in TR-DOS. Rename the 64 cpl driver for the same reason.
#
# 2016-08-11: Make also a 180 KiB disk 0 for +3DOS.
#
# 2016-10-22: Split the benchmarks disk into 3 disks, else it
# didn't fit in a TR-DOS disk image.
#
# 2016-11-16: Modify the cleaning of <tmp/>, to preserve
# <.gitignore>.
#
# 2016-11-20: Add background images.
#
# 2016-11-21: Adapt: tools used by make have been moved from
# <tools/> to <make/>.
#
# 2016-11-26: Fix lists of benchmark library files.
#
# 2016-12-08: Update after the renaming of "misc" and "common"
# module files.
#
# 2016-12-31: Remove the details about the background images.
#
# 2017-02-02: Make `gplusdos` the default rule.  Add Gforth to
# the requirements.
#
# 2017-02-02: Make `all` the default rule. Add a 1-letter
# shortcut for every DOS.
#
# 2017-02-06: Update: the <src/modules> directory has been
# renamed to <src/addons>, and <bin/modules> to <bin/addons>.
# The reason is to avoid confusion with library modules, in the
# documentation.
#
# 2017-02-09: Update the rules and filenames after the
# implementation of `set-block-disks`, which makes it possible
# to use several block disks, therefore making in unnecessary
# to copy the library in the additional disks. Reduce the
# number of disk images from 7 to 4.
#
# 2017-02-14: Fix the problem that made the BASIC loaders being
# rebuilt every time: the `.PHONY` instructions of kernel
# symbols targets in <Makefile.pasmo>. Fix also a similar
# problem with the <backgrounds/current.pbm> target: the
# solution was to do a copy instead of a link (hard or symbolic
# made no difference).
#
# 2017-02-15: Add rules to build the documentation from the
# sources.
#
# 2017-02-17: Don't make the documentation by default.
#
# 2017-02-18: Fix recipe aliases that build the documentation.
# Make the manual depend on <README.adoc>, from which some
# sections are taken.
#
# 2017-02-20: Build boot disk for Scorpion ZS 256. Update name
# of the source file of the manual. Add first rules to make a
# Info version of the manual. Build boot disks for Pentagon 512
# and Pentagon 1024.
#
# 2017-02-21: Add rules to create only the disk images of
# either Pentagon or Scorpion. Fix the contents of the
# workbench disk.
#
# 2017-02-24: Remove the experimental rules that build Info
# format manual. They are being tested apart.
#
# 2017-02-27: Update: use "fs" file extension instead of "fsb".
#
# 2017-03-04: Fix the rule that builds the +3DOS library disk.
#
# 2017-03-06: Add rule to build only the TR-DOS disks for 128k
# models.
#
# 2017-03-10: Add the kernel to the prerequisites of the
# patched loaders. This fixes a subtle bug: The patched loader
# was not updated the first time after a change in the kernel,
# even if the change implied a symbol. This made the system
# crash on TR-DOS when the `origin` address was modified,
# unless `make` was executed twice to force the loader be
# updated.
#
# 2017-03-12: Add draft rules to include the library, as a
# block file, in the boot disk images. Rename <bin/sys/> to
# </bin/dos/>.
#
# 2017-04-27: Rename the final HTML files of the manual. Update
# the `cleandoc` rule.
#
# 2017-05-07: Create a PDF from the HTML manual, using htmldoc.
#
# 2017-05-14: Remove the pr64.z80s addon, because it has been
# integrated into the library.
#
# 2017-05-15: Add 64-cpl fonts from:

# 	64#4 - 4x8 FONT DRIVER FOR 64 COLUMNS (c) 2007, 2011
# 	Original by Andrew Owen (657 bytes)
# 	Optimized by Crisis (602 bytes)
# 	Reimplemented by Einar Saukas (494 bytes)
# 	https://sites.google.com/site/zxgraph/home/einar-saukas/fonts
# 	http://www.worldofspectrum.org/infoseekid.cgi?id=0027130

# 2017-07-22: Add DocBook and EPUB experimental versions of the manual.
#
# 2017-12-05: Don't make a +3DOS 180 KiB boot disk anymore.
#
# 2017-12-07: Fix rule of +3DOS boot disk.
#
# 2018-02-27: Move editors from the library disk to the games disk and rename
# it, because the library didn't fit a TR-DOS disk image.
#
# 2018-03-10: Add `not_ready` to exclude modules under development. Needed for
# <src/lib/meta.test.forth2012-test-suite.fs>, which is being adapted.
#
# 2018-04-05: Remove the old code that made the disks containing also the
# library. Fix name of TR-DOS disk 1.
#
# 2018-04-07: Update after the renaming of program modules (games, block
# editors and `edit-sound`).
#
# 2018-04-10: Replace `htmldoc` with `asciidoctor-pdf` for making the PDF
# versions of the manual.
#
# 2018-04-11: Create gzipped PDF.
#
# 2018-04-17: Link the Forth words of the manual to the glossary, using the new
# `--annex` option of Glosara. Fix: make the +3DOS and TR-DOS manuals depend
# also on the Z80 flags notation document.
#
# 2018-06-04: Split the TR-DOS library into two disks.
#
# 2018-06-10: Rename TR-DOS phony recipes.
#
# 2018-06-12: Add taptools to the list of requirements.
#
# 2018-06-15: Prepare the list of exception codes that will be included in the
# manual.
#
# 2018-06-16: Finish the exception codes lists.
#
# 2018-07-20: Convert exception codes into tables instead of definition lists.
#
# 2018-07-21: Add `--attribute DOS` to Asciidoctor commands, to build the
# manuals using Asciidoctor's conditional preprocessor directives. Add
# `[%autowidth] to the tables of exception codes. Set an Asciidoctor attribute
# "dosname" instead of replacing the "%DOS%" mark with `sed`.
#
# 2018-07-22: Add a header to the tables of exception codes.
#
# 2018-07-29: Rename the TR-DOS disk image used for 128-KiB machines.
#
# 2020-02-27: Build EPUB versions of the manuals.
#
# 2020-02-28: Add the _Z80 instructions_ annex to the manual.
#
# 2020-02-29: Make the zip and gzip rules common to all cases. Generalize the
# interface to build the manual in any format for any DOS. Split the long
# command lines by parameters.  Get the Solo Forth version only once, store it
# in a variable and pass it to Asciidoctor as a parameter, instead of replacing
# a markup in the source file. Add Vim folding markers. Build PDF also with
# Pandoc (and wkhtmltopdf as PDF-engine). Build also a ODT version of the
# manuals.
#
# 2020-03-01: Build EPUB also with asciidoctor-epub3. Fix prerequites: README
# and manual skeleton.
#
# 2020-03-03: Fix prerequisite: exceptions file.
#
# 2020-04-03: Replace "docbook" filename extension with "dbk".
#
# 2020-04-05: Deprecate the PDF build by Pandoc from HTML. Simplify the
# filenames of the manuals built by Asciidoctor EPUB3 and Asciidoctor PDF. Fix
# the building of DocBook (the DOS label attribute was not passed, and the
# corresponding Asciidoctor conditions failed, making the DOS-spcific contents
# missing). Dont't build OpenDocument by default. Add rules to build the
# DocBook directly. Build an HTML manual also with Pandoc.
#
# 2020-04-06: Split the rules to build EPUB and HTML: one rule for every DOS
# and converter.
#
# 2020-04-13: Add `--epub-chapter-level` to Pandoc, to force the huge glossary
# XHTML file be splitted in subchapters into the EPUB.
#
# 2020-04-17: Test attribute `epub-chapter-level` to Asciidoctor EPUB3, just
# added to the future version following v0.5.0.alpha.15.
#
# 2020-04-27: Add attribute `epub-chapter-level`, after the fixes in
# Asciidoctor EPUB3 v0.5.0.alpha.17.dev.
#
# 2020-05-04: Add the apps to the glossary (actually, only `edit-sound`).
#
# 2020-05-05: Add the editors to the glossary.
#
# 2020-06-04: Add the tests to the glossary.
#
# 2020-06-17: Save error output of Asciidoctor EPUB3 to log files, to detect
# wrong cross-references. Add rule "epuba" to build the EPUBs only with
# Asciidoctor EPUB3.
#
# 2020-07-13: Fix compressed versions of PDF: make gzip keep the input files.
# Add titles to the exception codes tables.
#
# 2020-10-01: Compress only with gzip (deactivate zip); remove the uncompressed
# originals; compress also HTML. Deprecate HTML built from DocBook by Pandoc:
# the glossary IDs are lost in the process and the links don't work.
#
# 2020-10-02: Keep the ungzipped originals, e.g. HTML and PDF, in order to make
# updating faster. Build release zip archives containing the disks and manuals
# of every DOS. Make the master Asciidoctor manual depend also on the version
# number source file.
#
# 2020-10-03: Build a zip archive containing the sources. Improve the zip
# archives packing the contents into a release-specific directory.

# ==============================================================

# vim: foldmethod=marker
