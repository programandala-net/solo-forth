  \ tool.list.blocks.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201703062142

  \ -----------------------------------------------------------
  \ Description

  \ Words to list blocks.

  \ -----------------------------------------------------------
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ -----------------------------------------------------------
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ -----------------------------------------------------------
  \ Latest changes

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

( /line# .line# .line list-line list-lines list )

[unneeded] /line# ?\ : /line# ( -- n ) #16 base @ - 4 / 1+ ;

  \ doc{
  \
  \ /line# ( -- # )
  \
  \ Maximum length of a line number in the current radix.
  \ It works for decimal, hex and binary.
  \
  \ }doc

[unneeded] .line#
?\ need /line#  : .line# ( n -- ) /line# .r ;

  \ doc{
  \
  \ .line# ( n -- )
  \
  \ Print line number _n_ right-aligned in a field whose width
  \ depends on the current radix (decimal, hex or binary).
  \
  \ }doc

[unneeded] .line
?\ : .line ( n1 n2 -- ) line>string -trailing type ;

  \ doc{
  \
  \ .line ( n1 n2 -- )
  \
  \ Print line _n1_ from block _n2_, without trailing spaces.
  \
  \ Origin: fig-Forth.
  \
  \ }doc

[unneeded] list-line ?( need .line# need .line

: list-line ( n u -- ) cr over .line# space .line ; ?)

  \ doc{
  \
  \ list-line ( n u -- )
  \
  \ List line _n_ from block _u_, without trailing spaces.
  \
  \ See also: `list-lines`, `.line#`, `list`.
  \
  \ }doc

[unneeded] list-lines ?(
need .line need nuf? need list-line need ?leave
: list-lines ( u n1 n2 -- )
  rot dup scr ! cr ." Block " u.  1+ swap
  ?do  i scr @ list-line nuf? ?leave  loop cr ; ?)

  \ doc{
  \
  \ list-lines ( u n1 n2 -- )
  \
  \ List lines _n2..n3_ of block _u_ and store _u_ in `scr`.
  \
  \ See also: `list`, `scr`, `list-line`.
  \
  \ }doc

[unneeded] list ?( need list-lines
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
  \ See also: `scr`, `list-lines`, `lt`, `lm`, `lb`.
  \
  \ }doc

( /block# view .block# .index index )

[unneeded] /block# ?\ 3 constant /block#  exit

[unneeded] view ?( need locate need list

: view ( "name" -- ) locate dup 0= #-286 ?throw list ; ?)

  \ doc{
  \
  \ view ( "name" -- )
  \
  \ List the block where _name_ is defined, i.e. the first
  \ block where _name_ is in the index line (surrounded by
  \ spaces). If _name_ can not be found, throw exception #-286
  \ ("not located").
  \
  \ See also: `locate`, `list`.
  \
  \ }doc

[unneeded] .block# ?( need /block#
: .block# ( n -- ) /block# .r ; ?)

[unneeded] .index ?( need .line
: .index ( u -- )
  cr dup .block# space 0 swap .line ; ?)

  \ doc{
  \
  \ .index ( u -- )
  \
  \ Print the first line of the block _u_, which conventionally
  \ contains a comment with a title.
  \
  \ }doc

[unneeded] index ?( need .line need nuf? need ?leave
: index ( u1 u2 -- )
  1+ swap ?do  cr i .block# space 0 i .line  nuf? ?leave
  loop ; ?)

  \ doc{
  \
  \ index ( u1 u2 -- )
  \
  \ Print the first line of each block over the range from
  \ _u1_ to _u2_, which conventionally contains a comment with
  \ a title.
  \
  \ Origin: fig-Forth, Forth-79 (Reference Word Set), Forth-83
  \ (Uncontrolled Reference Words).
  \
  \ }doc

( index-like index-ilike )

need .index need contains need nuf? need ?leave

[unneeded] index-like ?(

: index-like ( u1 u2 "name" -- )
  parse-name 2swap
  1+ swap ?do  0 i line>string 2over contains
               if  i .index  then  nuf? ?leave
  loop  2drop ; ?)

  \ doc{
  \
  \ index-like ( u1 u2 "name" -- )
  \
  \ Print the first line of each block over the range from _u1_
  \ to _u2_, which conventionally contains a comment with a
  \ title, as long as the string _name_ is included in the
  \ line. The string comparison is case-sensitive.
  \
  \ See also: `index`, `index-ilike`.
  \
  \ }doc

[unneeded] index-ilike ?( need uppers

: index-ilike ( u1 u2 "name" -- )
  parse-name save-string 2dup uppers
  2swap 1+ swap ?do
    save-string  0 i line>string save-string 2dup uppers
    2over contains if  i .index  then
    nuf? ?leave
  loop  2drop ; ?)

  \ doc{
  \
  \ index-ilike ( u1 u2 "name" -- )
  \
  \ Print the first line of each block over the range from _u1_
  \ to _u2_, which conventionally contains a comment with a
  \ title, as long as the string _name_ is included in the
  \ line. The string comparison is case-insensitive.
  \
  \ See also: `index`, `index-like`.
  \
  \ }doc

  \ Note: The parsed string is re-saved to the circular string
  \ buffer in every iteration in order to prevent it from being
  \ overwritten by the strings of the index lines, because the
  \ circular string buffer is small.

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
  \ qx-columns ( -- n )
  \
  \ _n_ is the number of columns (2..4) of the quick index. It
  \ depends on the columns (32, 42, 64...) of the current
  \ screen mode.
  \
  \ See also: `qx`, `/qx-column`.
  \
  \ }doc

: /qx-column ( -- n ) columns qx-columns / ;

  \ doc{
  \
  \ /qx-column ( -- n )
  \
  \ _n_ is the width of a column of the quick index. It depends
  \ on the columns (32, 42, 64...) of the current screen mode.
  \
  \ See also: `qx`, `qx-columns`.
  \
  \ }doc

: /qx ( -- n ) rows 2- qx-columns * ;

  \ doc{
  \
  \ /qx ( -- n )
  \
  \ _n_ is the number of header lines shown on a quick index.
  \ It depends on the rows and columns of the current screen
  \ mode.
  \
  \ See also: `qx`.
  \
  \ }doc

: qx-bounds ( -- u1 u2 )
  scr @ /qx / /qx * /qx bounds 0 max swap blk/disk min swap ;

  \ doc{
  \
  \ qx-bounds ( -- u1 u2 )
  \
  \ Blocks to be included in the quick index, from block _u2_
  \ to block _u1-1_. They depend on `scr`.
  \
  \ See also: `qx`.
  \
  \ }doc

  \ need j need inverse
  \
  \ : qx ( -- ) home  qx-bounds do  qx-columns 0 do i j +  dup
  \ .block#  dup scr @ = abs inverse block /qx-column /block# -
  \ type  0 inverse loop  cr  qx-columns +loop ;
  \
  \ XXX OLD -- The stepped outer loops makes the block count
  \ overflow at the end of the disk, beyond `blk/disk`. The
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
  \ qx ( -- )
  \
  \ Give a quick index. The number and width of the columns
  \ depend on the current screen mode. The current block,
  \ stored in `scr`, is highlighted.
  \
  \ Origin: Gforth's ``blocked`` editor.
  \
  \ See also: `nx`, `px`.
  \
  \ }doc

: nx ( -- )
  /qx scr @ + [ blk/disk 1- ] literal min scr ! qx ;

  \ doc{
  \
  \ nx ( -- )
  \
  \ Give next quick index, calculated from `scr`.
  \
  \ See also: `qx`, `px`.
  \
  \ }doc

: px ( -- ) scr @ /qx - 0 max scr ! qx ;

  \ doc{
  \
  \ px ( -- )
  \
  \ Give previous quick index, calculated from `scr`.
  \
  \ See also: `qx`, `nx`.
  \
  \ }doc

( lt lm lb )

need list-lines

: lt ( -- ) scr @ 0 [ l/scr 2 / 1- ] literal list-lines ;

  \ doc{
  \
  \ lt ( -- )
  \
  \ List top half of screen hold in `scr`.
  \
  \ See also: `lm`, `lb`, `list`, `list-lines`.
  \
  \ }doc

: lm ( -- ) scr @ [ l/scr 4 / ] literal
                    [ l/scr 4 / 3 * 1- ] literal list-lines ;

  \ doc{
  \
  \ lm ( -- )
  \
  \ List middle part of screen hold in `scr`.
  \
  \ See also: `lt`, `lb`, `list`, `list-lines`.
  \
  \ }doc

: lb ( -- ) scr @ [ l/scr 2 / ] literal
                    [ l/scr 1-  ] literal list-lines ;

  \ doc{
  \
  \ lb ( -- )
  \
  \ List bottom half of screen hold in `scr`.
  \
  \ See also: `lt`, `lm`, `list`, `list-lines`.
  \
  \ }doc

  \ vim: filetype=soloforth
