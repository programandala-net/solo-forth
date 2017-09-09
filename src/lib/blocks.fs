  \ blocks.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201709091154
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to disk blocks.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( ?--> update flush thru )

[unneeded] ?-->
?\ : ?--> ( f -- ) if  postpone -->  then ; immediate

  \ doc{
  \
  \ ?--> ( f -- )
  \
  \ If _f_ is not false, continue interpretation on the next
  \ sequential block.  parse area. ``?-->`` is used for
  \ conditional compilation.
  \
  \ ``?-->`` is an `immediate` word.
  \
  \ See also: `-->`, `?(`, `?\`.
  \
  \ }doc

[unneeded] update
?\ : update ( -- ) disk-buffer @ $8000 or disk-buffer ! ;

  \ doc{
  \
  \ update ( -- )
  \
  \ Mark the current block buffer as modified.  The block will
  \ subsequently be transferred automatically to disk should
  \ its buffer be required for storage of a different block, or
  \ upon execution of `flush` or `save-buffers`.
  \
  \ Origin: Forth-83 (Required Word Set), Forth-94 (BLOCK),
  \ Forth-2012 (BLOCK).
  \
  \ }doc

[unneeded] flush
?\ : flush ( -- ) save-buffers empty-buffers ;

  \ doc{
  \
  \ flush ( -- )
  \
  \ Perform the function of `save-buffers`, then unassign all
  \ block buffers.
  \
  \ Origin: Forth-83 (Required Word Set), Forth-94 (BLOCK),
  \ Forth-2012 (BLOCK).
  \
  \ See also: `empty-buffers`.
  \
  \ }doc

[unneeded] thru
?\ : thru ( block1 block2 -- ) 1+ swap ?do  i load  loop ;
  \ XXX FIXME -- when block1>block2

  \ doc{
  \
  \ thru ( block1 block2 -- )
  \
  \ Load consecutively the blocks from _block1_ through
  \ _block2_.
  \
  \ Origin: Forth-79 (Reference Word Set), Forth-83
  \ (Controlled Reference Words), Forth-94 (BLOCK EXT),
  \ Forth-2012 (BLOCK EXT).
  \
  \ See also: `load`, `+thru`.
  \
  \ }doc

( continued ?load reload loads +load +thru loader )

[unneeded] continued ?\ : continued ( u -- ) ?loading (load) ;

  \ doc{
  \
  \ continued ( u -- )
  \
  \ Continue interpretation at block _u_.
  \
  \ Origin: Forth-79 (Reference Word Set), Forth-83 (Appendix
  \ B. Uncontrolled Reference Words).
  \
  \ See also: `-->`, `load`.
  \
  \ }doc

[unneeded] ?load
?\ : ?load ( block f -- ) if  dup load  then  drop ;

  \ Credit:
  \
  \ Code from Pygmy Forth.

  \ doc{
  \
  \ ?load ( u f -- )
  \
  \ Load block _u_ if flag _f_ is true, else do nothing.
  \
  \ Origin: Pygmy Forth.
  \
  \ See also: `load`.
  \
  \ }doc

[unneeded] reload
?\ : reload ( -- ) empty-buffers  lastblk @ load ;

  \ doc{
  \
  \ reload ( -- )
  \
  \ Load the most recently loaded block.
  \
  \ See also: `load`, `lastblk`.
  \
  \ }doc

[unneeded] loads ?\ : loads ( u n -- ) bounds ?do i load loop ;

  \ Credit:
  \
  \ Word from MMSFORTH.

  \ doc{
  \
  \ loads ( u n -- )
  \
  \ Load _n_ blocks starting from block _u_.
  \
  \ Origin: MMSFORTH.
  \
  \ }doc

[unneeded] +load  [unneeded] +thru  and
?\ : +load ( n -- ) blk @ + load ;

  \ doc{
  \
  \ +load ( n -- )
  \
  \ Load the block that is _n_ blocks from the current one.
  \
  \ See also: `load`, `blk`, `+thru`.
  \
  \ }doc

[unneeded] +thru
?\ : +thru ( u1 u2 -- ) 1+ swap ?do  i +load  loop ;

  \ doc{
  \
  \ +thru ( u1 u2 -- )
  \
  \ Load consecutively the blocks that are _u1_ blocks through
  \ _u2_ blocks from the current one.
  \
  \ See also: `+load`, `blk`, `load`.
  \
  \ }doc

[unneeded] loader
?\ : loader ( u "name" -- ) create , does> ( dfa ) @ load ;

  \ doc{
  \
  \ loader ( u "name" -- )
  \
  \ Define a word _name_ which, when executed, will  load block
  \ _u_.
  \
  \ Origin: Forth-79's ``loads`` (Reference Word Set),
  \ Forth-83's ``loads`` (Appendix B. Uncontrolled Reference
  \ Words).
  \
  \ }doc

( lineblock>source lineload load-program )

[unneeded] lineblock>source [unneeded] lineload and
?\ : lineblock>source ( n u -- ) blk !  c/l * >in ! ;

  \ doc{
  \
  \ lineblock>source ( n u -- )
  \
  \ Set block _u_ as the current source, starting from its
  \ line _n_.
  \
  \ See also: `block>source`.
  \
  \ }doc

[unneeded] lineload ?(

: lineload ( n u -- )
  dup 0= #-259 ?throw
  nest-source lineblock>source interpret unnest-source ; ?)

  \ doc{
  \
  \ lineload ( n u -- )
  \
  \ Begin interpretation at line _n_ of block _u_.
  \
  \ Origin: Forth-83 (Uncontrolled Reference Words).
  \
  \ See also: `load`.
  \
  \ }doc

[unneeded] load-program ?( need locate

variable loading-program

  \ doc{
  \
  \ loading-program ( -- a )
  \
  \ _a_ is the address of a cell containing a flag: Is a
  \ program being loaded by `load-program`?  This flag is
  \ modified by `load-program` and `end-program`.
  \
  \ }doc

: end-program ( -- ) loading-program off ; end-program

  \ doc{
  \
  \ end-program ( -- )
  \
  \ Mark the end of a program that is being loaded by
  \ `load-program`.
  \
  \ See also: `loading-program`.
  \
  \ }doc

  \ : load-program ( "name" -- )
  \   loading-program on
  \   blocks/disk locate ?do   loading-program @ 0= ?leave  i load
  \                      loop  end-program ; ?)
  \   \ XXX OLD -- incompatible with words that use `refill`

: (load-program ( u -- )
  blk !  loading-program on
  begin  loading-program @  blk @ blocks/disk <  and
  while  blk @ (load) 1 blk +!
  repeat end-program ; ?)

  \ doc{
  \
  \ (load-program ( u -- )
  \
  \ Load a program, i.e. a set of blocks that are loaded as a
  \ whole. The blocks of a program don't have block headers.
  \ Therefore programs can not have internal requisites, i.e.
  \ they use `need` only to load from the library, which must
  \ be before the blocks of the program on the disk or disks.
  \
  \ Programs don't need `-->` or any similar word to control
  \ the loading of blocks.  The loading starts from block _u_
  \ and continues until the last block of the disk or until
  \ `end-program` is executed.
  \
  \ ``(load-program`` is a factor of `load-program`.
  \ ``(load-program`` can be used to resume the `load-program`
  \ after an error, provided the code of block where the error
  \ happened (`lastblk`) is not the continuation of the
  \ previous block.
  \
  \ See also: `loading-program`.
  \
  \ }doc

: load-program ( "name" -- ) locate (load-program ;

  \ doc{
  \
  \ load-program ( "name" -- )
  \
  \ Load a program, i.e. a set of blocks that are loaded as a
  \ whole. The blocks of a program don't have block headers
  \ except the first one, which contains _name_. Therefore
  \ programs can not have internal requisites, i.e. they use
  \ `need` only to load from the library, which must be before
  \ the blocks of the program on the disk or disks.
  \
  \ Programs don't need `-->` or any similar word to control
  \ the loading of blocks: The loading starts from the first
  \ block of the disk that has _name_ in its header (surrounded
  \ by spaces), and continues until the last block of the disk
  \ or until `end-program` is executed.
  \
  \ See also: `loading-program`, `(load-program`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015..2016: Main development.
  \
  \ 2016-04-29: Add `lineload` and `lineblock>source`.
  \
  \ 2016-05-02: Join two blocks to save space.
  \
  \ 2016-05-13: Add `load-app`.
  \
  \ 2016-10-04: Start a new version of `load-app`, compatible
  \ with `[if]` and other words that use `refill`.
  \
  \ 2016-10-11: Finish the new version of `load-app`,
  \ compatible with `[if]` and other words that use `refill`.
  \
  \ 2016-11-21: Move `.line` to <tool.list.blocks.fsb>.
  \
  \ 2016-11-26: Rename `blocks` to `blk/disk` after the fix in
  \ the kernel.
  \
  \ 2016-11-27: Use `lastblk` in `reload`, instead of `scr`.
  \
  \ 2017-02-16: Remove `?\`, which is in the kernel.
  \
  \ 2017-02-17: Update cross references.  Change markup of
  \ inline code that is not a cross reference.
  \
  \ 2017-02-20: Replace `do`, which has been moved to the
  \ library, with `?do`.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-03-13: Update names including "rec" to "sector(s)";
  \ update names including "blk" to "block(s)".
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-04-01: Fix documentation.
  \
  \ 2017-05-08: Rename `load-app` to `load-program`, and so the
  \ related words. Improve documentation.
  \
  \ 2017-05-22: Factor `load-program` into `(load-program`, to
  \ make it possible to continue loading from a block, in case
  \ of error. Improve documentation.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".

  \ vim: filetype=soloforth
