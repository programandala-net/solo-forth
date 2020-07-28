  \ tool.turnkey.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to save the system.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( extend system-size system-zone turnkey )

  \ XXX WARNING -- Since name fields are kept in a memory bank,
  \ the best way to save a modified Forth system is creating a
  \ snapshot with a ZX Spectrum emulator, or with the
  \ equivalent feature provided by certain interfaces or modern
  \ ZX Spectrum clones.  ``turnkey`` and its related words are
  \ meant to save a Forth program that does not need to search
  \ the dictionary or use data already stored in paged memory.

  \ XXX TODO -- Study how to save and load the names bank, even
  \ after assembling the kernel.

: extend ( -- ) get-order only forth
                root-wordlist @      $06 +origin !
                forth-wordlist @     $08 +origin !
                assembler-wordlist @ $0A +origin !
                last-wordlist @      $0C +origin !
                width @              $18 +origin !
                latest               $1C +origin !
                here                 $1E +origin !
                latestxt             $22 +origin !
                np@                  $26 +origin !
                  \ XXX Needed?
                np@                  $28 +origin !
                set-order ;

  \ doc{
  \
  \ extend ( -- )
  \
  \ Change the `cold` start parameters to extend the system to
  \ its current state.
  \
  \ WARNING: This word is experimental. See the source code for
  \ details.
  \
  \ See also: `system-zone`, `system-size`, `turnkey`.
  \
  \ }doc

: system-size ( -- len ) here 0 +origin - ;

  \ doc{
  \
  \ system-size ( -- len )
  \
  \ _len_ is the size of the system, in bytes, i.e. the size of
  \ data/code space.
  \
  \ See also: `+origin`, `system-zone`, `turnkey`, `here`.
  \
  \ }doc

: system-zone ( -- a len ) 0 +origin system-size ;

  \ doc{
  \
  \ system-zone ( -- a len )
  \
  \ Return the start address _a_ of the system and its length
  \ _len_, to be used as parameters for saving the system to
  \ tape or disk.
  \
  \ See also: `+origin`, `system-size`, `turnkey`.
  \
  \ }doc

: turnkey ( xt -- a len ) ['] boot defer! extend system-zone ;

  \ doc{
  \
  \ turnkey ( xt -- a len )
  \
  \ Prepare the system in order to save a copy of its current
  \ state.  Return its start address _a_ and length _len_, to
  \ be used as parameters for saving the system to disk.  The
  \ saved copy will execute _xt_ after the ordinary boot
  \ process.
  \
  \ Usage example:

  \ ----
  \ ' my-game turnkey s" my-game" >tape-file
  \ ----

  \ WARNING: This word is experimental. See the source code for
  \ details.
  \
  \ WARNING: Since name fields are kept in a memory bank, the
  \ best way to save a modified Forth system is creating a
  \ snapshot with a ZX Spectrum emulator, or with the
  \ equivalent feature provided by certain interfaces or modern
  \ ZX Spectrum clones.  ``turnkey`` and its related words are
  \ meant to save a Forth program that does not need to search
  \ the dictionary or use data already stored in paged memory.
  \
  \ See also: `boot`, `extend`, `system-zone`, `cold`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-11-13: Rename `np@` to `hp@` after the changes in the
  \ kernel.
  \
  \ 2017-01-06: Update `voc-link` to `latest-wordlist`.
  \
  \ 2017-02-26: Update "hp" notation to "np", after the changes
  \ in the kernel.
  \
  \ 2018-03-09: Improve documentation.
  \
  \ 2018-04-15: Improve documentation of `size`. Deactivate the
  \ documentation.
  \
  \ 2018-06-09: Fix `turnkey`. Rename `size` to `system-size`.
  \ Rename `system` to `system-zone`.
  \
  \ 2018-06-10: Update and complete `extend`.
  \
  \ 2020-02-27: Improve documentation.
  \
  \ 2020-05-04: Improve and activate the documentation.
  \
  \ 2020-05-05: Fix cross reference. Improve documentation.
  \
  \ 2020-06-08: Update: rename `latest-wordlist` to
  \ `last-wordlist`.

  \ vim: filetype=soloforth
