# Solo Forth

## Description

Solo Forth is a Forth system for the ZX Spectrum 128 and compatible
computers, with disk drives and +3DOS, G+DOS, or TR-DOS.

Solo Forth cannot run on the original ZX Spectrum 48, but could be used
to develop programs for it.

Solo Forth can be used as a stand-alone Forth system (either in an
emulator or on the real computer), or as part of a cross-development
environment in a GNU/Linux operating system (in theory, other type of
operating systems could be used as well).

### Main features

- Fast DTC (Direct Threaded Code) implementation.

- A kernel as small as possible.

- Name space in banked memory, separated from code and data space.

- Easy access to banked memory.

- Big [library](#_library) of useful source code.

- Modular [DOS support](#_platforms).

- Fully documented source code.

- Detailed documentation.

- Conform to the [Forth standard](http://forth-standard.org) (not fully
  tested yet).

### Minimum requirements

- 128 KiB RAM.

- One double-sided 80-track disk drive (two or three recommended,
  depending on the DOS).

## Motivation, history and current status

The motivation behind Solo Forth is double:

1.  **I wanted to program the ZX Spectrum with a modern Forth system**:
    In 2015, my [detailed disassembly of ZX Spectrum’s Abersoft
    Forth](http://programandala.net/en.program.abersoft_forth.html), a
    popular tape-based implementation of fig-Forth ported to several
    8-bit home computers in the 1980’s (and the Forth system I started
    learning Forth with in 1984), helped me understand the inner working
    of the fig-Forth model, including its by-design limitations,
    compared to modern Forths, and discover some bugs of the ZX Spectrum
    port. At the same time I wrote the [Afera
    library](http://programandala.net/en.program.afera.html) in order to
    make Abersoft Forth more stable, powerful and comfortable for cross
    development. The objective was reached but, after a certain point,
    further improvements weren’t feasible without making radical changes
    in the system. The need for a new Forth system arised: a Forth
    designed from the start to use disk drives and banked memory, and
    useful for cross-development.

2.  **Nobody had written such a Forth system before:** In 2015 there was
    no disk-based Forth for the ZX Spectrum platform, and the only Forth
    written for ZX Spectrum 128 (the first model with banked memory) was
    Lennart Benschop’s Forth-83 (1988). But despite being more powerful
    than fig-Forth, it is still tape-based and keeps the block sources
    in a RAM disk. Besides, the system is built by metacompilation, what
    makes it difficult to adapt to disk drives.

The development of Solo Forth started on 2015-05-30, from the
disassembled code of Abersoft Forth. Some ideas and code were reused
also from the Afera library and from a previous abandoned project called
[DZX-Forth](http://programandala.net/en.program.dzx-forth.html) (a port
of CP/M DX-Forth to ZX Spectrum +3e).

On 2016-03-13 a Git repository was created from the development backups,
in order to preserve the evolution of the code from the start, and
uploaded to GitHub. On 2020-12-05 the Git repository was converted to
[Fossil](https://fossil-scm.org), keeping [GitHub as a
mirror](http://github.com/programandala-net/solo-forth). On 2023-04-06
the repository was converted to [Mercurial](https://mercurial-scm.org),
enabling a better interaction with GitHub. On 2023-09-12 the Mercurial
repository was [published on
Sourcehut](https://hg.sr.ht/~programandala_net/solo_forth), keeping
GitHub as a mirror.

Solo Forth is very stable, and it’s being used to develop two projects
in Forth: [Nuclear Waste
Invaders](http://programandala.net/en.program.nuclear_waste_invaders.html)
and [Black Flag](http://programandala.net/en.program.black_flag.html).

## Platforms

<table>
<caption>Supported platforms</caption>
<colgroup>
<col style="width: 33%" />
<col style="width: 33%" />
<col style="width: 33%" />
</colgroup>
<thead>
<tr>
<th style="text-align: left;">Computer</th>
<th style="text-align: left;">Disk interface</th>
<th style="text-align: left;">DOS</th>
</tr>
</thead>
<tbody>
<tr>
<td style="text-align: left;"><p>Pentagon 128</p></td>
<td style="text-align: left;"></td>
<td style="text-align: left;"><p>TR-DOS</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>Pentagon 512</p></td>
<td style="text-align: left;"></td>
<td style="text-align: left;"><p>TR-DOS</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>Pentagon 1024</p></td>
<td style="text-align: left;"></td>
<td style="text-align: left;"><p>TR-DOS</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>Scorpion ZS 256</p></td>
<td style="text-align: left;"></td>
<td style="text-align: left;"><p>TR-DOS</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>ZX Spectrum 128</p></td>
<td style="text-align: left;"><p>Beta 128</p></td>
<td style="text-align: left;"><p>TR-DOS</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>ZX Spectrum 128</p></td>
<td style="text-align: left;"><p>Plus D</p></td>
<td style="text-align: left;"><p>G+DOS</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>ZX Spectrum +2</p></td>
<td style="text-align: left;"><p>Beta 128</p></td>
<td style="text-align: left;"><p>TR-DOS</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>ZX Spectrum +2</p></td>
<td style="text-align: left;"><p>Plus D</p></td>
<td style="text-align: left;"><p>G+DOS</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>ZX Spectrum +2A</p></td>
<td style="text-align: left;"><p>(External disk drive)</p></td>
<td style="text-align: left;"><p>+3DOS</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>ZX Spectrum +2B</p></td>
<td style="text-align: left;"><p>(External disk drive)</p></td>
<td style="text-align: left;"><p>+3DOS</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>ZX Spectrum +3</p></td>
<td style="text-align: left;"></td>
<td style="text-align: left;"><p>+3DOS</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>ZX Spectrum +3e</p></td>
<td style="text-align: left;"></td>
<td style="text-align: left;"><p>+3DOS</p></td>
</tr>
</tbody>
</table>

## Project directories

<table>
<colgroup>
<col style="width: 15%" />
<col style="width: 17%" />
<col style="width: 67%" />
</colgroup>
<thead>
<tr>
<th style="text-align: left;">Directory</th>
<th style="text-align: left;">Subdirectory</th>
<th style="text-align: left;">Description</th>
</tr>
</thead>
<tbody>
<tr>
<td style="text-align: left;"><p>backgrounds</p></td>
<td style="text-align: left;"></td>
<td style="text-align: left;"><p>Version background images</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>bin</p></td>
<td style="text-align: left;"></td>
<td style="text-align: left;"><p>ZX Spectrum binary files for disk
0</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>bin</p></td>
<td style="text-align: left;"><p>addons</p></td>
<td style="text-align: left;"><p>Code loaded from disk, not assembled in
the library yet</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>bin</p></td>
<td style="text-align: left;"><p>dos</p></td>
<td style="text-align: left;"><p>DOS files</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>bin</p></td>
<td style="text-align: left;"><p>fonts</p></td>
<td style="text-align: left;"><p>Fonts for the supported screen
modes</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>disks</p></td>
<td style="text-align: left;"></td>
<td style="text-align: left;"><p>Disk images</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>disks</p></td>
<td style="text-align: left;"><p>gplusdos</p></td>
<td style="text-align: left;"><p>G+DOS disk images</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>disks</p></td>
<td style="text-align: left;"><p>plus3dos</p></td>
<td style="text-align: left;"><p>+3DOS disk images</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>disks</p></td>
<td style="text-align: left;"><p>trdos</p></td>
<td style="text-align: left;"><p>TR-DOS disk images</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>doc</p></td>
<td style="text-align: left;"></td>
<td style="text-align: left;"><p>Manuals in DocBook, EPUB, HTML and
PDF</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>make</p></td>
<td style="text-align: left;"></td>
<td style="text-align: left;"><p>Files used by <code>make</code> to
build the system</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>screenshots</p></td>
<td style="text-align: left;"></td>
<td style="text-align: left;"><p>Version screenshots</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>src</p></td>
<td style="text-align: left;"></td>
<td style="text-align: left;"><p>Sources</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>src</p></td>
<td style="text-align: left;"><p>addons</p></td>
<td style="text-align: left;"><p>Code to be loaded from disk. Not used
yet.</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>src</p></td>
<td style="text-align: left;"><p>doc</p></td>
<td style="text-align: left;"><p>Files used to build the
documentation</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>src</p></td>
<td style="text-align: left;"><p>inc</p></td>
<td style="text-align: left;"><p>Z80 symbols</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>src</p></td>
<td style="text-align: left;"><p>lib</p></td>
<td style="text-align: left;"><p>Library</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>src</p></td>
<td style="text-align: left;"><p>loader</p></td>
<td style="text-align: left;"><p>BASIC loader for disk 0</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>tmp</p></td>
<td style="text-align: left;"></td>
<td style="text-align: left;"><p>Temporary files created by
<code>make</code></p></td>
</tr>
<tr>
<td style="text-align: left;"><p>tools</p></td>
<td style="text-align: left;"></td>
<td style="text-align: left;"><p>Development and user tools</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>vim</p></td>
<td style="text-align: left;"></td>
<td style="text-align: left;"><p>Vim files</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>vim</p></td>
<td style="text-align: left;"><p>ftplugin</p></td>
<td style="text-align: left;"><p>Filetype plugin</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>vim</p></td>
<td style="text-align: left;"><p>syntax</p></td>
<td style="text-align: left;"><p>Syntax highlighting</p></td>
</tr>
</tbody>
</table>

## Disks

The \<disks\> directory of the [directory tree](#_tree) contains the
disk images:

    disks/*/disk_0_boot.*
    disks/*/disk_1*_library.*
    disks/*/disk_2_programs.*
    disks/*/disk_3_workbench.*

The subdirectory and the filename extension of every DOS are the
following:

<table>
<caption>DOS subdirectories and disk image filename extensions</caption>
<colgroup>
<col style="width: 17%" />
<col style="width: 31%" />
<col style="width: 51%" />
</colgroup>
<thead>
<tr>
<th style="text-align: left;">DOS</th>
<th style="text-align: left;">Subdirectory</th>
<th style="text-align: left;">Filename extension</th>
</tr>
</thead>
<tbody>
<tr>
<td style="text-align: left;"><p>+3DOS</p></td>
<td style="text-align: left;"><p>plus3dos</p></td>
<td style="text-align: left;"><p>dsk</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>G+DOS</p></td>
<td style="text-align: left;"><p>gplusdos</p></td>
<td style="text-align: left;"><p>mgt</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>TR-DOS</p></td>
<td style="text-align: left;"><p>trdos</p></td>
<td style="text-align: left;"><p>trd</p></td>
</tr>
</tbody>
</table>

## How to run

### In +3DOS

#### On ZX Spectrum +3/+3e

1.  Run a ZX Spectrum emulator and select a ZX Spectrum +3 (or [ZX
    Spectrum +3e](http://www.worldofspectrum.org/zxplus3e/)). Make sure
    its disk drives are configured as double-sided and 80-track in the
    emulator.

2.  “Insert” the disk image file \<disks/plus3dos/disk_0_boot.dsk\> as
    disk 'A'.

3.  Choose “Loader” from the computer start menu. Solo Forth will be
    loaded from disk.

### In G+DOS

#### On ZX Spectrum 128/+2 with the Plus D interface

1.  Run a ZX Spectrum emulator and select a ZX Spectrum 128 (or ZX
    Spectrum +2) with the Plus D disk interface. Make sure its disk
    drives are configured as double-sided and 80-track in the emulator.

2.  “Insert” the disk image file \<disks/gplusdos/disk_0_boot.mgt\> as
    disk 1 of the Plus D disk interface.

3.  Choose "128 BASIC" from the computer start menu.

4.  Type `run` in BASIC. G+DOS will be loaded from disk, and Solo Forth
    as well.

### In TR-DOS

<div class="important">

<div class="title">

</div>

The TR-DOS version of Solo Forth uses numbers as disk drive identifiers
(the same numbers TR-DOS uses internally) instead of the letters used by
the TR-DOS BASIC interface:

<table>
<caption>TR-DOS disk drive identifiers</caption>
<colgroup>
<col style="width: 33%" />
<col style="width: 33%" />
<col style="width: 33%" />
</colgroup>
<thead>
<tr>
<th style="text-align: left;">Drive</th>
<th style="text-align: left;">In TR-DOS</th>
<th style="text-align: left;">In Solo Forth</th>
</tr>
</thead>
<tbody>
<tr>
<td style="text-align: left;"><p>1st</p></td>
<td style="text-align: left;"><p>A</p></td>
<td style="text-align: left;"><p>0</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>2nd</p></td>
<td style="text-align: left;"><p>B</p></td>
<td style="text-align: left;"><p>1</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>3rd</p></td>
<td style="text-align: left;"><p>C</p></td>
<td style="text-align: left;"><p>2</p></td>
</tr>
<tr>
<td style="text-align: left;"><p>4th</p></td>
<td style="text-align: left;"><p>D</p></td>
<td style="text-align: left;"><p>3</p></td>
</tr>
</tbody>
</table>

</div>

#### On Pentagon 128

1.  Run a ZX Spectrum emulator and select a Pentagon 128. Make sure its
    disk drives are configured as double-sided and 80-track in the
    emulator.

2.  “Insert” the disk image file \<disks/trdos/disk_0_boot.128.trd\> as
    disk 'A'.

3.  Choose “TR-DOS” from the computer start menu. This will enter the
    TR-DOS command line[1].

4.  Press the 'R' key to get the `RUN` command and press the Enter key.
    Solo Forth will be loaded from disk.

#### On Pentagon 512

1.  Run a ZX Spectrum emulator and select a Pentagon 512. Make sure its
    disk drives are configured as double-sided and 80-track in the
    emulator.

2.  “Insert” the disk image file
    \<disks/trdos/disk_0_boot.pentagon_512.trd\> as disk 'A'.

3.  Choose "128k menu"[2] from the computer start menu (the reset
    service menu). This will enter a ZX Spectrum 128 style menu. Choose
    “TR-DOS”. This will enter the TR-DOS command line.

4.  Press the 'R' key to get the `RUN` command and press the Enter key.
    Solo Forth will be loaded from disk.

#### On Pentagon 1024

1.  Run a ZX Spectrum emulator and select a Pentagon 1024. Make sure its
    disk drives are configured as double-sided and 80-track in the
    emulator.

2.  “Insert” the disk image file
    \<disks/trdos/disk_0_boot.pentagon_1024.trd\> as disk 'A'.

3.  Choose "128k menu" from the computer start menu (the reset service
    menu). This will enter a ZX Spectrum 128 style menu. Choose
    “TR-DOS”. This will enter the TR-DOS command line.

4.  Press the 'R' key to get the `RUN` command and press the Enter key.
    Solo Forth will be loaded from disk.

#### On Scorpion ZS 256

1.  Run a ZX Spectrum emulator and select a Scorpion ZS 256. Make sure
    its disk drives are configured as double-sided and 80-track in the
    emulator.

2.  “Insert” the disk image file
    \<disks/trdos/disk_0_boot.scorpion_zs_256.trd\> as disk 'A'.

3.  Choose "128 TR-DOS" from the computer start menu. Solo Forth will be
    loaded from disk.

#### On ZX Spectrum 128/+2 with the Beta 128 interface

1.  Run a ZX Spectrum emulator and select a ZX Spectrum 128 (or ZX
    Spectrum +2) with the Beta 128 interface. Make sure its disk drives
    are configured as double-sided and 80-track in the emulator.

2.  “Insert” the disk image file \<disks/trdos/disk_0_boot.128.trd\> as
    disk A of the Beta 128 interface.

3.  Choose "128 BASIC" from the computer start menu.

4.  Type `randomize usr 15616` in BASIC (or just `run usr15616` to save
    seven keystrokes). This will enter the TR-DOS command line.

5.  Press the 'R' key to get the `RUN` command and press the Enter key.
    Solo Forth will be loaded from disk.

## How to use the library

### In +3DOS

1.  [Run Solo Forth](#_run_plus3dos).

2.  “Insert” the file \<disks/plus3dos/disk_1_library.dsk\> as disk B.
    `'b' set-drive throw` to make drive 'B' the current one.

3.  Type `1 load` to load block 1 from the library disk. By convention,
    block 0 cannot be loaded (it is used for comments), and block 1 is
    used as a loader. In Solo Forth, block 1 contains `2 load`, in order
    to load the `need` tool from block 2.

4.  Type `need name`, were “name” is the name of the word or tool you
    want to load from the library.

### In G+DOS

1.  [Run Solo Forth](#_run_gplusdos).

2.  “Insert” the file \<disks/gplusdos/disk_1_library.mgt\> as disk 2 of
    the Plus D disk interface. Type `2 set-drive throw` to make drive 2
    the current one.

3.  Type `1 load` to load block 1 from the library disk. By convention,
    block 0 cannot be loaded (it is used for comments), and block 1 is
    used as a loader. In Solo Forth, block 1 contains `2 load`, in order
    to load the `need` tool from block 2.

4.  Type `need name`, were “name” is the name of the word or tool you
    want to load from the library.

### In TR-DOS

1.  [Run Solo Forth](#_run_trdos).

2.  “Insert” the file \<disks/trdos/disk_1a_library.trd\> into the first
    disk drive (called A in TR-DOS and 0 in Solo Forth), and the file
    \<disks/trdos/disk_1b_library.trd\> into the second disk drive
    (called B in TR-DOS and 1 in Solo Forth).

    Notice that the library is split into two disks because the maximun
    capacity of a TR-DOS disk is only 640 KiB.

    Also remember in Solo Forth the [TR-DOS disk drive
    identifiers](#trdosdiskdrives) are numbers 0..3 instead of letters
    A..D.

3.  Type `1 load` to load block 1 from the first library disk. By
    convention, block 0 cannot be loaded (it is used for comments), and
    block 1 is used as a loader. In Solo Forth, block 1 contains
    `2 load`, in order to load the `need` tool from block 2.

4.  Type `need 2-block-drives` to load and execute the word
    `2-block-drives` from the library, setting the first two drives as
    block drives in their normal order.

5.  Type `need name`, were “name” is the name of the word or tool you
    want to load from the library.

## Documentation

The \<doc\> directory contains one version of the manual for every
supported DOS, EPUB, HTML and PDF formats. The manuals are built
automatically from the sources and other files. At the moment they
contain a description of the Forth system, the basic information
required to use it and a complete glossary with cross references.

[1] The TR-DOS command line uses keyboard tokens, like the ZX Spectrum
48, but commands typed in 'L' cursor mode will be recognized as well, as
on the ZX Spectrum 128 editor. In order to get the 'L' cursor mode you
can type a quote (Symbol Shift + 'P') or press 'E' to get keyword `REM`.
When the DOS command is typed in full, the quote or the `REM` must be
removed from the start of the line before pressing 'Enter'.

[2] In theory, choosing option “TR-DOS” from the system service menu
should work. But it seems it depends on a specific version of TR-DOS.
This alternative method is longer, but it works with the TR-DOS 5.03
ROM.
