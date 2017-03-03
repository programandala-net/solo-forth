  \ dos.COMMON.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702220020

  \ -----------------------------------------------------------
  \ Description

  \ Code common to any DOS.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ History

  \ 2017-02-08: Start. First version of `set-block-drives` and
  \ related words.
  \
  \ 2017-02-09: Fix `(>block-drive`: `drive` is not needed,
  \ because `(block-drives` contains the actual identifiers,
  \ not the ordinal numbers. Fix and factor the check of drives
  \ number in `set-block-drives`.  Update `last-locatable` in
  \ `set-block-drives`.
  \
  \ 2017-02-17: Update cross references.

( drive )

: drive ( c1 -- c2 ) first-drive + ;

  \ doc{
  \
  \ drive ( c1 -- c2 )
  \
  \ Convert drive number _c1_ (0 index) to the range required
  \ for the current DOS.
  \
  \ See also: `first-drive`.
  \
  \ }doc

( set-block-drives )

create (block-drives ( -- ca ) max-drives allot

$FF cconstant not-block-drive

: -block-drives ( -- )
  (block-drives max-drives not-block-drive fill ;

: >block-drive ( n -- ca ) (block-drives + ;

: block-drive@ ( n -- c ) >block-drive c@ ;

: block-drive! ( c n -- c ) >block-drive c! ;

: ?drives ( u -- ) max-drives > #-287 ?throw ;

: set-block-drives ( c[u]..c[1] u -- )
  dup ?drives -block-drives
  dup block-drives c!  max-blocks 1- last-locatable !
      0 ?do  i block-drive!  loop ;

[undefined] g+dos ?\ 2 1      2 set-block-drives  -->
[undefined] tr-dos ?\ 3 2 1 0  4 set-block-drives  -->
[undefined] +3dos ?\ 'B' 'A'  2 set-block-drives  -->

( set-block-drives )

: ?drive# ( u -- )
  [ max-drives 1- ] 1literal > #-35 ?throw ;

: ?block-drive ( u -- ) not-block-drive = #-35 ?throw ;

: (>drive-block ( u1 -- u2 )
  blk/disk /mod ( block drive ) dup ?drive#
  block-drive@ dup ?block-drive set-drive throw ;
  \ Convert block _u1_ to its equivalent _u2_ in its corresponding
  \ disk drive, which is set the current drive.

' (>drive-block ' >drive-block defer!

  \ vim: filetype=soloforth