  \ display.mode.42rs.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 202005051417
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ A 42-CPL display mode.

  \ ===========================================================
  \ Authors

  \ Author of the 42-CPL printing routine: Ricardo Serral Wigge.
  \ Published on Microhobby, issue 66 (1986-02), page 24:
  \ http://microhobby.org/numero066.htm
  \ http://microhobby.speccy.cz/mhf/066/MH066_24.jpg

  \ Marcos Cruz (programandala.net) integrated it into Solo
  \ Forth, 2015, 2016, 2017, 2018, 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ ===========================================================
  \ To-do

  \ XXX TODO -- integrate the source of the driver

  \ XXX TODO -- check how the UDG are printed (8 pixels width?)

  \ XXX FIXME -- a pixel of the cursor is not deleted when
  \ backspace is used on the command line

( mode-42rs banked-mode-42rs )

need mode-32 need (mode-42rs need set-mode-output
need get-drive need drive need file>

: mode-42rs ( -- ) [ latestxt ] literal current-mode !
                  (mode-42rs set-mode-output ;

  \ doc{
  \
  \ mode-42rs ( -- ) "mode-42-r-s"
  \
  \ Start the 42-CPL display mode based on a routine written by
  \ Ricardo Serral Wigge, published on Microhobby, issue 66
  \ (1986-02), page 24:

  \ - http://microhobby.org/numero066.htm
  \ - http://microhobby.speccy.cz/mhf/066/MH066_24.jpg

  \ WARNING: ``mode-42rs`` is under development. See the source
  \ code for details.
  \
  \ See: `current-mode`, `mode-42pw`.
  \
  \ }doc

get-drive  0 drive set-drive throw
           s" pr42.bin" 0 0 file> throw  \ load the driver
           s" ea5a.f42" 0 0 file> throw  \ load the font
set-drive throw

( banked-mode-42rs )

  \ XXX UNDER DEVELOPMENT -- A variant of `mode-42rs` that stores
  \ the driver and the font in the code bank.

  \ XXX FIXME -- crash!

need mode-32 need (mode-42rs
need drive need get-drive need file>

need set-banked-mode-output need code-bank

: banked-mode-42rs ( -- ) [ latestxt ] literal current-mode !
                         (mode-42rs set-banked-mode-output ;

code-bank{  get-drive 0 drive set-drive throw
                        s" pr42.bin" 0 0 file> throw
                        s" ea5a.f42" 0 0 file> throw
            set-drive throw }code-bank
  \ Load the driver and the font into the code bank.

( (mode-42rs )

need columns need rows need set-font need (at-xy

: mode-42rs-xy ( -- col row ) 0 0 ;  \ XXX TODO

: (mode-42rs ( -- a )
  42 to columns  24 to rows
  ['] mode-42rs-xy ['] xy defer!
  ['] (at-xy ['] at-xy defer!
  [ 64600 256 - ] literal set-font 63900 ;
  \ Set the 42-CPL font and `at-xy`;
  \ return the address of the output routine.

  \ ===========================================================
  \ Change log

  \ 2016-04-26: Update `latest name>` to `latestxt`.
  \
  \ 2016-05-07: Compact the blocks. Improve the file header.
  \ Fix: `need mode-32` was missing.
  \
  \ 2017-04-21: Rename module and words after the new
  \ convention for display modes.
  \
  \ 2016-08-11: Rename the filenames of the driver and the
  \ font.
  \
  \ 2016-10-16: Typo.
  \
  \ 2016-11-17: Use `?(` instead of `[if]`.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-01-20: Fix typo.
  \
  \ 2017-02-08: Update the usage of `set-drive`, which now
  \ returns an error result.
  \
  \ 2017-02-11: Replace `<file-as-is` with `0 0 file>`, after
  \ the improvements in the G+DOS module. Use `drive` to make
  \ the code compatible with any DOS.
  \
  \ 2017-04-21: Rename module and words after the new
  \ convention for display modes. Need `(at-xy`, which has been
  \ moved to the common module.
  \
  \ 2018-01-24: Update after the renaming of all display modes
  \ files and words: "42" -> "42rs" (Ricardo Serral).
  \
  \ 2020-05-05: Document `mode-42rs`. Improve documentation.

  \ vim: filetype=soloforth
