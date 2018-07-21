  \ tool.list.blocks.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201807212114
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to list blocks.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( /line# .line# .line list-line list-lines list )

unneeding /line# ?\ : /line# ( -- n ) #16 base @ - 4 / 1+ ;

  \ doc{
  \
  \ /line# ( -- n ) "slash-line-hash"
  \
  \ Maximum length of a line number in the current radix.
  \ It works for decimal, hex and binary.
  \
  \ See: `.line#`.
  \
  \ }doc

unneeding .line# ?\ need /line# : .line# ( n -- ) /line# .r ;

  \ doc{
  \
  \ .line# ( n -- ) "dot-line-hash"
  \
  \ Display line number _n_ right-aligned in a field whose
  \ width depends on the current radix (decimal, hex or
  \ binary).
  \
  \ See: `/line#`.
  \
  \ }doc

unneeding .line

?\ : .line ( n1 n2 -- ) line>string -trailing type ;

  \ doc{
  \
  \ .line ( n1 n2 -- ) "dot-line"
  \
  \ Display line _n1_ from block _n2_, without trailing spaces.
  \
  \ Origin: fig-Forth.
  \
  \ See: `.line#`, `blk-line`.
  \
  \ }doc

unneeding list-line ?( need .line# need .line

: list-line ( n u -- ) cr over .line# space .line ; ?)

  \ doc{
  \
  \ list-line ( n u -- )
  \
  \ List line _n_ from block _u_, without trailing spaces.
  \
  \ See: `list-lines`, `.line#`, `.line`. `list`, `blk-line`.
  \
  \ }doc

unneeding list-lines ?( need .line need nuf?
                        need list-line need ?leave

: list-lines ( u n1 n2 -- )
  rot dup scr ! cr ." Block " u.  1+ swap
  ?do  i scr @ list-line nuf? ?leave  loop cr ; ?)

  \ doc{
  \
  \ list-lines ( u n1 n2 -- )
  \
  \ List lines _n2..n3_ of block _u_ and store _u_ in `scr`.
  \
  \ See: `list`, `scr`, `list-line`.
  \
  \ }doc

unneeding list ?( need list-lines

: list ( u -- ) dup max-blocks 1- u> #-35 ?throw
                    0 [ l/scr 1- ] literal list-lines ; ?)

  \ doc{
  \
  \ list ( u -- )
  \
  \ Display block _u_ and store _u_ in `scr`.
  \
  \ Origin: fig-Forth, Forth-79 (Required Word Set), Forth-83
  \ (Controlled Reference Words), Forth-94 (BLOCK EXT),
  \ Forth-2012 (BLOCK EXT).
  \
  \ See: `scr`, `list-lines`, `lt`, `lm`, `lb`.
  \
  \ }doc

( /block# view .block# .index index blk-line )

unneeding /block# ?\ 3 constant /block#

unneeding view ?( need locate need list

: view ( "name" -- ) locate dup 0= #-286 ?throw list ; ?)

  \ doc{
  \
  \ view ( "name" -- )
  \
  \ List the block where _name_ is defined, i.e. the first
  \ block where _name_ is in the index line (surrounded by
  \ spaces). If _name_ can not be found, `throw` an exception
  \ #-286 ("not located").
  \
  \ See: `locate`, `list`.
  \
  \ }doc

unneeding .block# ?( need /block#

: .block# ( n -- ) /block# .r ; ?)

unneeding .index ?( need .line need .block#

: .index ( u -- ) cr dup .block# space 0 swap .line ; ?)

  \ doc{
  \
  \ .index ( u -- ) "dot-index"
  \
  \ Display the first line of the block _u_, which
  \ conventionally contains a comment with a title.
  \
  \ }doc

unneeding index ?( need .line need nuf? need ?leave
                   need .block#

: index ( u1 u2 -- )
  1+ swap ?do  cr i .block# space 0 i .line  nuf? ?leave
  loop ; ?)

  \ doc{
  \
  \ index ( u1 u2 -- )
  \
  \ Display the first line of each block over the range from
  \ _u1_ to _u2_, which conventionally contains a comment with
  \ a title.
  \
  \ Origin: fig-Forth, Forth-79 (Reference Word Set), Forth-83
  \ (Uncontrolled Reference Words).
  \
  \ }doc

unneeding blk-line ?(

: blk-line ( -- ca len )
  blk @ block >in @ dup c/l mod - + c/l ; ?)

  \ doc{
  \
  \ blk-line ( -- ca len )
  \
  \ Return the current line _ca len_ of the block being
  \ interpreted.  No check is done whether any block is
  \ actually being interpreted.
  \
  \ See: `blk`, `block`, `>in/l`, `->in/l`, `c/l`.
  \
  \ }doc

( index-like index-ilike )

need .index need contains need nuf? need ?leave

unneeding index-like ?(

: index-like ( u1 u2 "name" -- )
  parse-name 2swap
  1+ swap ?do  0 i line>string 2over contains
               if  i .index  then  nuf? ?leave
  loop  2drop ; ?)

  \ doc{
  \
  \ index-like ( u1 u2 "name" -- )
  \
  \ Display the first line of each block over the range from
  \ _u1_ to _u2_, which conventionally contains a comment with
  \ a title, as long as the string _name_ is included in the
  \ line. The string comparison is case-sensitive.
  \
  \ See: `index`, `index-ilike`.
  \
  \ }doc

unneeding index-ilike ?( need uppers

: index-ilike ( u1 u2 "name" -- )
  parse-name >stringer 2dup uppers
  2swap 1+ swap ?do
    >stringer  0 i line>string >stringer 2dup uppers
    2over contains if  i .index  then
    nuf? ?leave
  loop  2drop ; ?)

  \ doc{
  \
  \ index-ilike ( u1 u2 "name" -- )
  \
  \ Display the first line of each block over the range from
  \ _u1_ to _u2_, which conventionally contains a comment with
  \ a title, as long as the string _name_ is included in the
  \ line. The string comparison is case-insensitive.
  \
  \ See: `index`, `index-like`.
  \
  \ }doc

  \ Note: The parsed string is re-saved to the `stringer` in
  \ every iteration in order to prevent it from being
  \ overwritten by the strings of the index lines, because the
  \ `stringer` is small.

( qx nx px )

  \ Quick index

  \ Credit:
  \
  \ Code extracted, adapted and improved from Gforth's
  \ `blocked` blocks editor, originally written by Bernd
  \ Paysan, 1995.

need rows need columns
need .line# need /line# need .block# need /block#

: qx-columns ( -- n ) columns 14 / ;

  \ doc{
  \
  \ qx-columns ( -- n ) "q-x-columns"
  \
  \ _n_ is the number of columns (2..4) of the quick index. It
  \ depends on the columns (32, 42, 64...) of the current
  \ screen mode.
  \
  \ See: `qx`, `/qx-column`.
  \
  \ }doc

: /qx-column ( -- n ) columns qx-columns / ;

  \ doc{
  \
  \ /qx-column ( -- n ) "slash-q-x-column"
  \
  \ _n_ is the width of a column of the quick index. It depends
  \ on the columns (32, 42, 64...) of the current screen mode.
  \
  \ See: `qx`, `qx-columns`.
  \
  \ }doc

: /qx ( -- n ) rows 2- qx-columns * ;

  \ doc{
  \
  \ /qx ( -- n ) "slash-q-x"
  \
  \ _n_ is the number of header lines shown on a quick index.
  \ It depends on the rows and columns of the current screen
  \ mode.
  \
  \ See: `qx`.
  \
  \ }doc

: qx-bounds ( -- u1 u2 ) scr @ /qx / /qx * /qx bounds
                         0 max swap blocks/disk min swap ;

  \ doc{
  \
  \ qx-bounds ( -- u1 u2 ) "q-x-bounds"
  \
  \ Blocks to be included in the quick index, from block _u2_
  \ to block _u1-1_. They depend on `scr`.
  \
  \ See: `qx`.
  \
  \ }doc

  \ need j need inverse
  \
  \ : qx ( -- ) home  qx-bounds do  qx-columns 0 do i j +  dup
  \ .block#  dup scr @ = abs inverse block /qx-column /block# -
  \ type  0 inverse loop  cr  qx-columns +loop ;
  \
  \ XXX OLD -- The stepped outer loops makes the block count
  \ overflow at the end of the disk, beyond `blocks/disk`. The
  \ next version uses one single loop:

  \ : qx-row? ( n -- f ) qx-columns mod 1+ qx-columns = ;
  \
  \ need j need inverse
  \
  \ : qx ( -- ) home  0  qx-bounds ?do
  \     i dup .block#  dup scr @ = inverse
  \     block /qx-column /block# - type  0 inverse
  \     dup qx-row? if  cr  then  1+  loop  drop ;
  \
  \ XXX OLD -- `cr` caused problems because the `mode64` driver
  \ increases the current line even when the cursor position is
  \ at the end of a line. That's not the way the default
  \ printing system works, `mode32`, or `mode42`.  Happily `cr`
  \ is not needed, as long as the columns fill the whole width
  \ of the screen, what the current calculations already do.
  \ The next version does not use `cr` at the end of the index
  \ rows:

  \ need inverse
  \
  \ : qx ( -- )
  \   home  qx-bounds ?do
  \     i .block#  i scr @ = inverse
  \     i block /qx-column /block# - type  0 inverse
  \   loop ;
  \
  \ XXX OLD -- `mode64` does not support `inverse`.  The next
  \ version uses a mark instead, so the current block is
  \ visible in any screen mode:

: qx ( -- )
  home  qx-bounds ?do
    i scr @ = if  '>' /block# emits  else  i .block#  then
    i block /qx-column /block# - type
  loop ;

  \ doc{
  \
  \ qx ( -- ) "q-x"
  \
  \ Give a quick index. The number and width of the columns
  \ depend on the current screen mode. The current block,
  \ stored in `scr`, is highlighted.
  \
  \ Origin: Gforth's ``blocked`` editor.
  \
  \ See: `nx`, `px`.
  \
  \ }doc

: nx ( -- )
  /qx scr @ + [ blocks/disk 1- ] literal min scr ! qx ;

  \ doc{
  \
  \ nx ( -- ) "n-x"
  \
  \ Give next quick index, calculated from `scr`.
  \
  \ See: `qx`, `px`.
  \
  \ }doc

: px ( -- ) scr @ /qx - 0 max scr ! qx ;

  \ doc{
  \
  \ px ( -- ) "p-x"
  \
  \ Give previous quick index, calculated from `scr`.
  \
  \ See: `qx`, `nx`.
  \
  \ }doc

( lt lm lb )

need list-lines

: lt ( -- ) scr @ 0 [ l/scr 2 / 1- ] literal list-lines ;

  \ doc{
  \
  \ lt ( -- ) "l-t"
  \
  \ List top half of screen hold in `scr`.
  \
  \ See: `lm`, `lb`, `list`, `list-lines`.
  \
  \ }doc

: lm ( -- ) scr @ [ l/scr 4 / ] literal
                  [ l/scr 4 / 3 * 1- ] literal list-lines ;

  \ doc{
  \
  \ lm ( -- ) "l-m"
  \
  \ List middle part of screen hold in `scr`.
  \
  \ See: `lt`, `lb`, `list`, `list-lines`.
  \
  \ }doc

: lb ( -- ) scr @ [ l/scr 2 / ] literal
                  [ l/scr 1-  ] literal list-lines ;

  \ doc{
  \
  \ lb ( -- ) "l-b"
  \
  \ List bottom half of screen hold in `scr`.
  \
  \ See: `lt`, `lm`, `list`, `list-lines`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-11-20: Factor `list-lines` from `list`. Improve
  \ documentation of all words. Compact the code to save one
  \ block. Add conditional compilation to `list`, `list-lines`,
  \ `index`, `.index`.
  \
  \ 2016-11-21: Modify title of listed screen, to avoid
  \ confusion when the radix is other than decimal. Move the
  \ `blocked` editor's quick index here, improve it and
  \ document it. Write `/line#` and `.line#`, useful factors of
  \ `list-lines`. Move `contains` to <strings.misc.fsb>.  Move
  \ `.line` from <blocks.fsb>. Compact the code, saving one
  \ block. Add `/block#` and `.block#`, for clarity.
  \
  \ 2016-11-22: Make the quick index work with any screen mode.
  \ Move `lt`, `lm` and `lb` from the `blocked` editor.
  \
  \ 2016-11-24: Factor `list-line` from `list-lines`.
  \
  \ 2016-11-26: Need `?leave`, which has been moved to the
  \ library.  Rename `blocks` to `blk/disk` after the fix in
  \ the kernel.
  \
  \ 2017-01-11: Add `view`.
  \
  \ 2017-01-17: Fix and improve documentation of `list`, `lt`,
  \ `lm` and `lb`.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-01-26: Change title of `list-lines` to "Block" and
  \ print its number as unsigned (no practical difference when
  \ using floppy disks, but block numbers are unsigned).
  \
  \ 2017-02-01: Add `need uppers`, because `uppers` has been
  \ moved to the library.
  \
  \ 2017-02-17: Update cross references.  Change markup of
  \ inline code that is not a cross reference.
  \
  \ 2017-03-06: Make `list` check the block number.  Block
  \ numbers beyond the current limit were managed properly by
  \ G+DOS, which throws error #-1004 (sector error); but +3DOS
  \ throws #-1004 (no data) and then any further attempt to
  \ access the disk throws error #-1000 (drive not ready; and
  \ TR-DOS exists to BASIC with "Disc error Trk X sec X
  \ Retry,Abort,Ignore?"... Beside, the heading of the block
  \ was shown anyway before the error. The check prevents all
  \ this.
  \
  \ 2017-03-12: Update the names of `stringer` words and
  \ mentions to it.
  \
  \ 2017-03-13: Update names including "rec" to "sector(s)";
  \ update names including "blk" to "block(s)".
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-11: Fix stack comment. Improve documentation. Move
  \ `blk-line` from the `ttester` module.
  \
  \ 2018-03-13: Fix requirement of `.index`.
  \
  \ 2018-03-14: Fix requirement of `index`.
  \
  \ 2018-07-21: Improve documentation, linking `throw`.

  \ vim: filetype=soloforth
