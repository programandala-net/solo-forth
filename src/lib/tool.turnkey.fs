  \ tool.turnkey.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 201702261232
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to save the system.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( extend size system turnkey )

  \ XXX WARNING -- Since name fields are saved in a memory
  \ bank, the best way to save a modified Forth system is to
  \ make a snapshot with the ZX Spectrum emulator; otherwise a
  \ multipart saving and loading would be needed.  Anyway,
  \ these words are meant to save a Forth program that does not
  \ need to search the dictionary.
  \
  \ XXX TODO -- Study how to save and load the names bank, even
  \ after assembling the kernel.

: extend ( -- )
  latest $08 +origin !
    \ top most word in `forth` vocabulary
  here $1F +origin !
    \ `dp` init value
  np@ $26 +origin !
    \ `np` init value
  latest-wordlist @ $0C +origin ! ;
    \ `latest-wordlist` init value
  \ XXX TODO -- update

  \ doc{
  \
  \ extend ( -- )
  \
  \ }doc
  \
  \ Change the `cold` start parameters to extend the system to
  \ its current state.

: size ( -- u ) here 0 +origin - ;

  \ doc{
  \
  \ size ( -- u )
  \
  \ Size of the system.
  \
  \ }doc

: system ( -- a len ) extend  0 +origin size 10 + ;

  \ doc{
  \
  \ system ( -- a len )
  \
  \ Prepare the system in order to save a copy.  Return its
  \ start address and length, to be used as parameters for
  \ saving the system to disk.
  \
  \ }doc

: turnkey ( xt -- a len ) boot defer! system ;

  \ doc{
  \
  \ turnkey ( xt -- a len )
  \
  \ Prepare the system in order to save a copy that will
  \ execute the given xt after the ordinary boot process.
  \ Return its start address and length, to be used as
  \ parameters for saving the system to disk.
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

  \ vim: filetype=soloforth
