  \ dos.COMMON.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703050017

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
  \
  \ 2017-03-04: Document `set-block-drives` and all related
  \ words.

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

  \ doc{
  \
  \ (block-drives  ( -- ca )
  \
  \ _ca_ is the address of a character table that holds the
  \ disk drives used as block drives. This table is configured
  \ by `set-block-drives`.  The length of the table is
  \ `max-drives`.  The first element of the table (offset 0) is
  \ the disk drive used for blocks from number 0 to number
  \ `blk/disk` minus one; the second element of the table
  \ (offset 1) the disk drive used for blocks from number
  \ `blk/disk` to number ``blk/disk 2 * 1-``; and so on.  The
  \ block ranges not associated to disk drives are marked with
  \ `not-block-drive`. The number of block drives is hold in
  \ `block-drives`.
  \
  \ }doc

  \ XXX TODO -- Rename.

$FF cconstant not-block-drive

  \ doc{
  \
  \ not-block-drive ( -- c )
  \
  \ _c_ is a constant identifier used by `set-block-drives`,
  \ `-block-drives` and other related words to mark unused
  \ elements of `(block-drives`.
  \
  \ }doc

: -block-drives ( -- )
  (block-drives max-drives not-block-drive fill ;

  \ doc{
  \
  \ -block-drives  ( -- )
  \
  \ Fill `(block-drives` with `not-block-drive`, making no disk
  \ drive be used as block drive.
  \
  \ See also: `set-block-drives`.
  \
  \ }doc

: >block-drive ( n -- ca ) (block-drives + ;

  \ XXX TODO -- Remove.

  \ doc{
  \
  \ >block-drive ( n -- ca )
  \
  \ Convert _n_ to its address in `(block-drives`.
  \
  \ See also: `block-drive@`, `block-drive!`.
  \
  \ }doc

: block-drive@ ( n -- c ) >block-drive c@ ;

  \ XXX TODO -- Remove.

  \ doc{
  \
  \ block-drive@ ( n -- c ) >block-drive c@ ;
  \
  \ Get drive _c_ currently used as block drive number _n_.
  \
  \ See also: `block-drive!`, `set-block-drives`,
  \ `>block-drive`.
  \
  \ }doc

: block-drive! ( c n -- ) >block-drive c! ;

  \ XXX TODO -- Remove.

  \ doc{
  \
  \ block-drive! ( c n -- ) >block-drive c@ ;
  \
  \ Set drive _c_ as block drive number _n_.
  \
  \ See also: `block-drive@`, `set-block-drives`,
  \ `>block-drive`.
  \
  \ }doc

: ?drives ( u -- ) max-drives > #-287 ?throw ;

  \ doc{
  \
  \ ?drives ( u -- )
  \
  \ If _u_ is greater than the maximum number of disk drives,
  \ throw exception #-287 ("wrong number of drives").
  \
  \ See also: `set-block-drives`.
  \
  \ }doc

: set-block-drives ( c[u]..c[1] u -- )
  dup ?drives -block-drives
  dup block-drives c!  max-blocks 1- last-locatable !
      0 ?do i block-drive! loop ;

  \ doc{
  \
  \ set-block-drives ( c[n]..c[1] n -- )
  \
  \ Set the block drives to the drives specified by drive
  \ identifiers _c[n]..c[1]_. Subsequently drive _c[1]_ will be
  \ searched first for blocks, from block 0 to `blk/disk` minus
  \ one, and so on.
  \
  \ If _n_ is zero, no drive is used for blocks.
  \
  \ When this word is loaded, the default configuration is set,
  \ i.e. use all drives for blocks:

  \ |===
  \ | DOS    | Default configuration
  \
  \ | G+DOS  | ``2 1      2 set-block-drives``
  \ | TR-DOS | ``3 2 1 0  4 set-block-drives``
  \ | +3DOS  | ``'B' 'A'  2 set-block-drives``
  \ |===

  \ NOTE: ``set-block-drives`` sets `last-locatable` to the
  \ last block available on the new configuration, but
  \ `first-locatable` is not modified.
  \
  \ See also: `-block-drives`, `block-drives`, `block-drive!`.
  \
  \ }doc

[undefined] g+dos  ?\ 2 1      2 set-block-drives  -->
[undefined] tr-dos ?\ 3 2 1 0  4 set-block-drives  -->
[undefined] +3dos  ?\ 'B' 'A'  2 set-block-drives  -->

( set-block-drives )

: ?drive# ( u -- )
  [ max-drives 1- ] 1literal > #-35 ?throw ;

  \ doc{
  \
  \ ?drive# ( u -- )
  \
  \ If _u_ is greater than the maximum number of disk drives,
  \ throw exception #-287 ("invaled block number").
  \
  \ See also: `set-block-drives`.
  \
  \ }doc

: ?block-drive ( u -- ) not-block-drive = #-35 ?throw ;

  \ doc{
  \
  \ ?block-drive# ( u -- )
  \
  \ If _u_ is `not-block-drive`, throw exception #-35 ("invalid
  \ block number").
  \
  \ See also: `set-block-drives`.
  \
  \ }doc

: (>drive-block ( u1 -- u2 )
  blk/disk /mod ( block drive ) dup ?drive#
  block-drive@ dup ?block-drive set-drive throw ;

  \ doc{
  \
  \ (>drive-block ( u1 -- u2 )
  \
  \ Convert block _u1_ to its equivalent _u2_ in its corresponding
  \ disk drive, which is set the current drive.
  \
  \ ``(>drive-block`` is the action of `>drive-block` after
  \ `set-block-drives` has been loaded.
  \
  \ }doc

' (>drive-block ' >drive-block defer!

( get-block-drives )

need set-block-drives

  \ XXX FIXME -- At the moment, `get-block-drives` depends on
  \ `set-block-drives`. The problem is `(block-drives`,
  \ contrary to `block-drives`, is not in the kernel. So, if
  \ `get-block-drives` was loaded before `set-block-drives`,
  \ and `(block-drives` could be required without
  \ `set-block-drives`, `block-drives` would contain 1 but
  \ `(block-drives` would be empty... The simplest solution is
  \ to move `(block-drives` to the kernel and combine it with
  \ `block-drive` into a single area.

: get-block-drives ( -- c[u]..c[1] u )
  block-drives c@ dup 0 ?do dup i - 1- block-drive@ swap loop ;

  \ doc{
  \
  \ get-block-drives ( -- c[n]..c[1] n )
  \
  \ Get the current configuration of block drives, as
  \ configured by `
  \
  \ to the drives specified by drive
  \ identifiers
  \ _c[n]..c[1]_. Subsequently drive _c[1]_ will be searched
  \ first for blocks, from block 0 to `blk/disk` minus one, and
  \ so on.
  \
  \ If _n_ is zero, no drive is used for blocks.
  \
  \ When this word is loaded, the default configuration is set,
  \ i.e. use all drives for blocks:

  \ |===
  \ | DOS    | Default configuration
  \
  \ | G+DOS  | ``2 1      2 set-block-drives``
  \ | TR-DOS | ``3 2 1 0  4 set-block-drives``
  \ | +3DOS  | ``'B' 'A'  2 set-block-drives``
  \ |===
  \
  \ See also: `-block-drives`, `block-drives`, `block-drive!`.
  \
  \ }doc

  \ vim: filetype=soloforth
