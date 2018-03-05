  \ dos.COMMON.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803052149
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Code common to any DOS.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( drive ?drive# ?block-drive block-drives )

unneeding drive ?\ : drive ( c1 -- c2 ) first-drive + ;

  \ doc{
  \
  \ drive ( c1 -- c2 )
  \
  \ Convert drive number _c1_ (0 index) to actual drive
  \ identifier _c2_ (DOS dependent).
  \
  \ ``drive`` is used in order to make the code portable,
  \ abstracting the DOS drive identifiers.
  \
  \ Usage example:

  \ ----
  \ \ Set the second disk drive as default:
  \
  \ 2 set-drive       \ on G+DOS only
  \ 1 set-drive       \ on TR-DOS only
  \ 'B' set-drive     \ on +3DOS only
  \
  \ 1 drive set-drive \ on any DOS -- portable code
  \ ----

  \ See: `first-drive`, `max-drives`.
  \
  \ }doc

unneeding ?drive# ?(

: ?drive# ( u -- )
  [ max-drives 1- ] xliteral u> #-35 ?throw ; ?)

  \ doc{
  \
  \ ?drive# ( u -- ) "question-drive-hash"
  \
  \ If _u_ is greater than the maximum number of disk drives,
  \ throw exception #-35 ("invalid block number").
  \
  \ See: `(>drive-block`, `block-drives`, `?block-drive`,
  \ `?drives`.
  \
  \ }doc

unneeding ?block-drive

?\ : ?block-drive ( u -- ) not-block-drive = #-35 ?throw ;

  \ doc{
  \
  \ ?block-drive# ( u -- ) "question-block-drive-hash"
  \
  \ If _u_ is `not-block-drive`, throw exception #-35 ("invalid
  \ block number").
  \
  \ See: `(>drive-block`, `block-drives`, `?drive#`,
  \ `?drives`.
  \
  \ }doc

unneeding block-drives ?( need not-block-drive

create block-drives ( -- ca ) max-drives allot
  block-drives max-drives not-block-drive fill
  first-drive block-drives c!

  \ doc{
  \
  \ block-drives ( -- ca )
  \
  \ _ca_ is the address of a character table that holds the
  \ disk drives used as block drives. This table can be
  \ configured manually or using `set-block-drives`.
  \
  \ The length of the table is `max-drives`.  The first element
  \ of the table (offset 0) is the disk drive used for blocks
  \ from number 0 to number ``blocks/disk 1-``; the second element
  \ of the table (offset 1) the disk drive used for blocks from
  \ number `blocks/disk` to number ``blocks/disk 2 * 1-``; and so on.
  \
  \ The number of used block drives is hold in `#block-drives`.
  \
  \ The block ranges not associated to disk drives are marked
  \ with $FF (the `not-block-drive` optional constant is
  \ provided for convenience), and all of them should be at the
  \ end of the table.  In theory it's possible to define gaps
  \ in the whole range of blocks associated to disk drives, but
  \ this would cause trouble with `set-block-drives` and
  \ `get-block-drives`, which use `#block-drives` as the drives
  \ count from the start of `block-drives`.
  \
  \ The default configuration of ``block-drives`` is: use only
  \ the first disk drive for blocks.
  \
  \ }doc

need ?drive# need block-drive@ need ?block-drive

: (>drive-block ( u1 -- u2 )
  blocks/disk /mod ( block drive ) dup ?drive#
  block-drive@ dup ?block-drive set-drive throw ;

  \ doc{
  \
  \ (>drive-block ( u1 -- u2 ) "paren-to-drive-block"
  \
  \ Convert block _u1_ to its equivalent _u2_ in its
  \ corresponding disk drive, which is set the current drive.
  \
  \ ``(>drive-block`` becomes the action of `>drive-block`
  \ after `block-drives` has been loaded.
  \
  \ See: `?drive#`, `?block-drive`, `set-drive`,
  \ `set-block-drives`.
  \
  \ }doc

' (>drive-block ' >drive-block defer! ?)

( not-block-drive -block-drives block-drive@ block-drive! )

unneeding not-block-drive ?\ $FF cconstant not-block-drive

  \ doc{
  \
  \ not-block-drive ( -- c )
  \
  \ _c_ is a constant identifier used by `set-block-drives`,
  \ `-block-drives` and other related words to mark unused
  \ elements of `block-drives`.
  \
  \ }doc

unneeding -block-drives ?( need block-drives
                            need not-block-drive

: -block-drives ( -- )
  block-drives max-drives not-block-drive fill ; ?)

  \ doc{
  \
  \ -block-drives ( -- ) "minus-block-drives"
  \
  \ Fill `block-drives` with `not-block-drive`, making no disk
  \ drive be used as block drive.
  \
  \ See: `set-block-drives`, `get-block-drives`.
  \
  \ }doc

unneeding block-drive@ ?( need block-drives

: block-drive@ ( n -- c ) block-drives + c@ ; ?)

  \ doc{
  \
  \ block-drive@ ( n -- c ) "block-drive-fetch"
  \
  \ Get drive _c_ (DOS dependent) currently used as block drive
  \ number _n_ (0 index).
  \
  \ See: `block-drive!`, `get-block-drives`.
  \
  \ }doc

unneeding block-drive@ ?( need block-drives

: block-drive! ( c n -- ) block-drives + c! ; ?)

  \ doc{
  \
  \ block-drive! ( c n -- ) "block-drive-store"
  \
  \ Set drive _c_ (DOS dependent) as block drive number _n_ (0
  \ index).
  \
  \ See: `block-drive@`, `set-block-drives`.
  \
  \ }doc

( ?drives set-block-drives get-block-drives )

unneeding ?drives

?\ : ?drives ( n -- ) max-drives > #-287 ?throw ;

  \ doc{
  \
  \ ?drives ( n -- ) "question-drives"
  \
  \ If _n_ is greater than the maximum number of disk drives,
  \ throw exception #-287 ("wrong number of drives").
  \
  \ See: `set-block-drives`.  `?block-drive`, `?drive#`.
  \
  \ }doc

unneeding set-block-drives ?(

need ?drives need -block-drives need block-drive!

: set-block-drives ( c#n..c#1 n -- )
  dup ?drives -block-drives
  dup #block-drives c!  max-blocks 1- last-locatable !
      0 ?do i block-drive! loop ; ?)

  \ doc{
  \
  \ set-block-drives ( c#n..c#1 n -- )
  \
  \ Set the block drives to the drives specified by drive
  \ identifiers _c#n..c#1_. Subsequently drive _c#1_ will be
  \ searched first for blocks from block number 0 to block
  \ number ``blocks/disk 1-``; drive _c[n+1]_ will be searched
  \ for blocks from block number `blocks/disk` to block number
  \ ``blocks/disk 2 * 1-``; and so on.
  \
  \ If _n_ is zero, no drive is used for blocks.
  \
  \ NOTE: ``set-block-drives`` sets `last-locatable` to the
  \ last block available on the new configuration, but
  \ `first-locatable` is not modified.
  \
  \ See: `-block-drives`, `#block-drives`, `block-drive!`,
  \ `get-block-drives`.
  \
  \ }doc

unneeding get-block-drives ?(

need block-drive@

: get-block-drives ( -- c#n..c#1 n )
  #block-drives c@
  dup 0 ?do dup i - 1- block-drive@ swap loop ; ?)

  \ doc{
  \
  \ get-block-drives ( -- c#n..c#1 n )
  \
  \ Get the current configuration of block drives, as
  \ configured by `
  \
  \ to the drives specified by drive identifiers _c#n..c#1_.
  \ Subsequently drive _c#1_ will be searched first for
  \ blocks, from block 0 to `blocks/disk` minus one, and so on.
  \
  \ If _n_ is zero, no drive is used for blocks.
  \
  \ See: `-block-drives`, `#block-drives`, `block-drive!`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

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
  \
  \ 2017-03-06: Rename `block-drives` to `#block-drives`;
  \ rename `(block-drives` to `block-drives`.  Reorganize the
  \ code to make `get-block-drives` independent from
  \ `set-block-drives`. Remove `>block-drive`. Improve
  \ documentation. Remove the default configuration of
  \ `block-drives` that was set when `set-block-drives` was
  \ compiled.
  \
  \ 2017-03-13: Update names including "rec" to "sector(s)";
  \ update names including "blk" to "block(s)".
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-03-28: Fix typo.
  \
  \ 2017-04-21: Fix stack notation of `?drives`.
  \
  \ 2017-12-05: Fix and update stack notation.
  \
  \ 2018-01-03: Update `1literal` to `xliteral`.
  \
  \ 2018-02-04: Fix documentation. Improve documentation: add
  \ pronunciation to words that need it.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.

  \ vim: filetype=soloforth
