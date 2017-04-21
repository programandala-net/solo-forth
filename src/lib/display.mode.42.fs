  \ display.mode.42.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 201704211658
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ A 42 CPL screen mode.

  \ ===========================================================
  \ Authors

  \ Author of the 42 cpl printing routine: Ricardo Serral Wigge.
  \ Published on Microhobby, issue 66 (1986-02), page 24:
  \ http://microhobby.org/numero066.htm
  \ http://microhobby.speccy.cz/mhf/066/MH066_24.jpg

  \ Marcos Cruz (programandala.net) integrated it into Solo
  \ Forth, 2015, 2016, 2017.

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

( mode-42 banked-mode-42 )

need mode-32 need (mode-42 need set-mode-output
need get-drive need drive need file>

: mode-42 ( -- ) [ latestxt ] literal current-mode !
                  (mode-42 set-mode-output ;
  \ Set the 42 cpl printing mode: the driver, the font
  \ and `at-xy`.

get-drive  0 drive set-drive throw
           s" pr42.bin" 0 0 file> throw  \ load the driver
           s" ea5a.f42" 0 0 file> throw  \ load the font
set-drive throw

( banked-mode-42 )

  \ XXX UNDER DEVELOPMENT -- A variant of `mode-42` that stores
  \ the driver and the font in the code bank.

  \ XXX FIXME -- crash!

need mode-32 need (mode-42
need drive need get-drive need file>

need set-banked-mode-output need code-bank

: banked-mode-42 ( -- ) [ latestxt ] literal current-mode !
                         (mode-42 set-banked-mode-output ;

code-bank{  get-drive 0 drive set-drive throw
                        s" pr42.bin" 0 0 file> throw
                        s" ea5a.f42" 0 0 file> throw
            set-drive throw }code-bank
  \ Load the driver and the font into the code bank.

( (mode-42 )

need columns need rows need set-font need (at-xy

: mode-42-xy ( -- col row ) 0 0 ;  \ XXX TODO

: (mode-42 ( -- a )
  42 to columns  24 to rows
  ['] mode-42-xy ['] xy defer!
  ['] (at-xy ['] at-xy defer!
  [ 64600 256 - ] literal set-font 63900 ;
  \ Set the 42 cpl font and `at-xy`;
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

  \ vim: filetype=soloforth
