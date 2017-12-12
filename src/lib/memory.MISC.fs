  \ memory.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201712121834
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words to manipulate memory.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( -! c+! c-! )

[unneeded] -! ?(

code -! ( n|u a -- )
  E1 c, D1 c, 7E c, 90 03 + c, 70 07 + c, 23 c,
    \ pop hl ; address
    \ pop de ; number
    \ ld a,(hl)
    \ sub a,e
    \ ld (hl),a
    \ inc hl
  7E c, 98 02 + c, 70 07 + c, jpnext, end-code ?)
    \ ld a,(hl)
    \ sbc a,d
    \ ld (hl),a
    \ _jp_next

  \ doc{
  \
  \ -! ( n|u a -- )
  \
  \ Subtract _n|u_ from the single-cell number stored at _a_.
  \
  \ See: `+!`, `1-!`, `c-!`.
  \
  \ }doc

[unneeded] c+! ?(

code c+! ( c ca -- )
  E1 c, D1 c, 78 03 + c, 86 c, 70 07 + c, jpnext, end-code ?)
    \ pop hl
    \ pop de
    \ ld a,e
    \ add a,(hl)
    \ ld (hl),a
    \ _jp_next

  \ doc{
  \
  \ c+! ( c ca - )
  \
  \ Add _c_ to the character stored at _ca_
  \
  \ See: `c-!`, `c1+!`, `+!`.
  \
  \ }doc

[unneeded] c-! ?(

code c-! ( c ca -- )
  E1 c, D1 c, 7E c, 90 03 + c, 70 07 + c, jpnext, end-code ?)
    \ pop hl
    \ pop de
    \ ld a,(hl)
    \ sub e
    \ ld (hl),a
    \ _jp_next

  \ doc{
  \
  \ c-! ( c ca - )
  \
  \ Subtract _c_ from the character stored at _ca_
  \
  \ See: `c+!`, `c1-!`, `-!`.
  \
  \ }doc

( c1+! c1-! 1+! 1-! )

[unneeded] c1+!

?\ code c1+! ( ca -- ) E1 c, 34 c, jpnext, end-code
    \ pop hl
    \ inc (hl)
    \ _jp_next

  \ doc{
  \
  \ c1+! ( ca - )
  \
  \ Increment the character stored at _ca_.
  \
  \ See: `c1-!`, `c+!`, `1+!`.
  \
  \ }doc

[unneeded] c1-!

?\ code c1-! ( ca -- ) E1 c, 35 c, jpnext, end-code
    \ pop hl
    \ dec (hl)
    \ _jp_next

  \ doc{
  \
  \ c1-! ( ca - )
  \
  \ Decrement the character stored at _ca_.
  \
  \ See: `c1+!`, `c-!`, `1-!`.
  \
  \ }doc

[unneeded] 1+! ?(

code 1+! ( a -- )
  E1 c, 5E c, 23 c, 56 c, 13 c, 70 02 + c, 2B c, 70 03 + c,
  jpnext, end-code ?)
    \ pop hl
    \ ld e,(hl)
    \ inc hl
    \ ld d,(hl)
    \ inc de
    \ ld (hl),d
    \ dec hl
    \ ld (hl),e
    \ _jp_next

  \ doc{
  \
  \ 1+! ( a - )
  \
  \ Increment the single-cell number stored at _a_.
  \
  \ See: `c1+!`, `1-!`, `+!`.
  \
  \ }doc

[unneeded] 1-! ?(

code 1-! ( a -- )
  E1 c, 5E c, 23 c, 56 c, 1B c, 70 02 + c, 2B c, 70 03 + c,
  jpnext, end-code ?)
    \ pop hl
    \ ld e,(hl)
    \ inc hl
    \ ld d,(hl)
    \ inc de
    \ ld (hl),d
    \ dec hl
    \ ld (hl),e
    \ _jp_next

  \ doc{
  \
  \ 1-! ( a - )
  \
  \ Decrement the single-cell number stored at _a_.
  \
  \ See: `1+!`, `c1-!`, `-!`.
  \
  \ }doc

( @+ 2@+ c@+ )

[unneeded] @+

?\ : @+ ( a -- a' x ) dup cell+ swap @ ;

  \ doc{
  \
  \ @+ ( a -- a' x )
  \
  \ Fetch _x_ from _a_. Return _a'_, which is _a_
  \ incremented by one cell.
  \ This is handy for stepping through cell arrays.
  \
  \ See: `@`, `2@+`, `c@+`.
  \
  \ }doc

[unneeded] 2@+

?\ : 2@+ ( a -- a' xd ) dup cell+ cell+ swap 2@ ;

  \ doc{
  \
  \ 2@+ ( a -- a' xd )
  \
  \ Fetch _xd_ from _a_. Return _a'_, which is _a_
  \ incremented by two cells.
  \ This is handy for stepping through double-cell arrays.
  \
  \ See: `2@`, `@+`, `c@+`.
  \
  \ }doc

[unneeded] c@+ ?\ need alias ' count alias c@+ ( ca -- ca' c )

  \ doc{
  \
  \ c@+ ( ca -- ca' c )
  \
  \ Fetch the character _c_ at _ca_. Return _ca'_, which is
  \ _ca_ incremented by one character.  This is handy for
  \ stepping through character arrays.
  \
  \ This word is an `alias` of `count`.
  \
  \ See: `c@`, `2@+`, `@+`.
  \
  \ }doc

( n, nn, n@ nn@ n! nn! )

[unneeded] n, ?\ : n, ( x[u]..x[1] u -- ) 0 ?do , loop ;

  \ doc{
  \
  \ n, ( x[u]..x[1] u -- )
  \
  \ If _u_ is not zero, store _u_ cells _x[u]..x[1]_ into data
  \ space, being _x[1]_ the first one stored and _x[u]_ the
  \ last one.
  \
  \ See: `,`, `nn,`, `n@`, `n!`.
  \
  \ }doc

[unneeded] nn, ?( need need-here need-here n,

: nn, ( x[u]..x[1] u -- ) dup , n, ; ?)

  \ doc{
  \
  \ nn, ( x[u]..x[1] u -- )
  \
  \ Store the count _u_ into data space.  If _u_ is not zero,
  \ store also _u_ cells _x[u]..x[1]_ into data space, being
  \ _x[1]_ the first one stored and _x[u]_ the last one.
  \
  \ See: `,`, `n,`, `nn!`.
  \
  \ }doc

[unneeded] n@ ?(

: n@ ( a u -- x[u]..x[1] )
  tuck 1- cells + \ point _a_ to _x[u]_
  swap 0 ?do dup i cells - @ swap loop drop ; ?)

  \ doc{
  \
  \ n@ ( a u -- x[u]..x[1] )
  \
  \ If _u_ is not zero, read _u_ cells _x[u]..x[1]_ from _a_,
  \ being _x[1]_ the first one stored and _x[u]_ the last one.
  \
  \ See: `nn@`, `@`, `nn!`.
  \
  \ }doc

[unneeded] nn@ ?( need need-here need-here n@

: nn@ ( a -- x[1]..x[u] u | 0 ) dup @ >r cell+ r@ n@ r> ; ?)

  \ doc{
  \
  \ nn@ ( a -- x[1]..x[u] u | 0 )
  \
  \ Read the count _u_ from _a_.  If it's zero, return it.  If
  \ _u_ is not zero, read _u_ cells _x[u]..x[1]_ from the next
  \ cell address, being _x[1]_ the first cell stored there and
  \ _x[u]_ the last one.
  \
  \ See: `n@`, `@`, `nn!`.
  \
  \ }doc

[unneeded] n! ?(

: n! ( x[u]..x[1] u a -- )
  swap 0 ?do dup >r ! r> cell+ loop drop ; ?)

  \ doc{
  \
  \ n! ( x[u]..x[1] u a -- )
  \
  \ If _u_ is not zero, store _u_ cells at address _a_, being
  \ _x[1]_ the first cell stored there and _x[u]_ the last one.
  \
  \ See: `nn!`, `!`, `n@`.
  \
  \ }doc

[unneeded] nn! ?( need need-here need-here n!

: nn! ( x[u]..x[1] u a -- ) 2dup ! cell+ n! ; ?)

  \ doc{
  \
  \ nn! ( x[u]..x[1] u a -- )
  \
  \ Store the count _u_ at _a_.  If _u_ is not zero, store also
  \ _u_ cells _x[u]..x[1]_ at the next cell address, being
  \ _x[1]_ the first one stored and _x[u]_ the last one.
  \
  \ See: `n!`, `!`, `nn@`.
  \
  \ }doc

( bit>mask bit? set-bit reset-bit )

[unneeded] bit>mask

?\ need lshift : bit>mask ( n -- b ) 1 swap lshift ;

  \ doc{
  \
  \ bit>mask ( n -- b )
  \
  \ Convert bit number _n_ to a bitmask _b_ with bit _n_ set.
  \
  \ See: `bit?`, `set-bit`, `reset-bit`.
  \
  \ }doc

[unneeded] bit?

?\ need bit>mask : bit? ( b n -- f ) bit>mask and 0<> ;

  \ doc{
  \
  \ bit? ( b n -- f )
  \
  \ Is bit _n_ of _b_ set?
  \
  \ See: `bit?`, `set-bit`, `bit>mask`.
  \
  \ }doc

[unneeded] set-bit?

?\ need bit>mask : set-bit ( b1 n -- b2 ) bit>mask or ;

  \ doc{
  \
  \ set-bit ( b1 n -- b2 )
  \
  \ Set bit _n_ of _b1_, returning the result _b2_.
  \
  \ See: `bit?`, `set-bit`, `bit>mask`.
  \
  \ }doc

[unneeded] reset-bit? ?( need bit>mask

: reset-bit ( b1 n -- b2 ) bit>mask invert and ; ?)

  \ doc{
  \
  \ reset-bit ( b1 n -- b2 )
  \
  \ Reset bit _n_ of _b1_, returning the result _b2_.
  \
  \ See: `bit?`, `set-bit`, `bit>mask`.
  \
  \ }doc

( c@and ctoggle coff )

  \ Credit:
  \
  \ Words inspired by MPE PowerForth for TiniARM.

  \ XXX OLD -- `c@and?` is in the kernel
  \ [unneeded] c@and? ?(
  \ code c@and? ( b ca -- f )
  \   hl pop  de pop  e a ld  m and
  \   ' true jpnz  ' false jp end-code ?)

  \ c@and? ( b ca -- f )
  \
  \ Fetch the caracter at _ca_ and do a bit-by-bit logical
  \ "and" of it with _b_. Return _false_ if the result
  \ is zero, else _true_.

[unneeded] c@and ?(

code c@and ( b1 ca -- b2 )
  E1 c, D1 c, 78 03 + c, A6 c, C3 c, pusha , jpnext,
    \ pop hl
    \ pop de
    \ ld a,e
    \ and (hl)
    \ jp push_a
  end-code ?)

  \ doc{
  \
  \ c@and ( b1 ca -- b2 )
  \
  \ Fetch the caracter at _ca_ and do a bit-by-bit logical
  \ `and` of it with _b1_, returning the result _b2_.
  \
  \ See: `c@and?`, `ctoggle`, `cset`, `creset`.
  \
  \ }doc

  \ XXX OLD -- `cset` is in the kernel
  \ [unneeded] cset ?(
  \ code cset ( b ca -- )
  \   hl pop  de pop  e a ld  m or  a m ld  jpnext,
  \ end-code ?)
  \   \ Set the bits at _ca_ specified by the bitmask _b_.

  \ XXX OLD -- `creset` is in the kernel
  \ [unneeded] creset ?(
  \ code creset ( b ca -- )
  \   hl pop  de pop  e a ld  cpl  m and  a m ld  jpnext,
  \ end-code ?)
  \   \ Reset the bits at _ca_ specified by the bitmask _b_.

[unneeded] ctoggle ?(

code ctoggle ( b ca -- )
  E1 c, D1 c, 7E c, A8 03 + c, 70 07 + c, jpnext, end-code ?)
    \ pop hl
    \ pop de
    \ ld a,(hl)
    \ xor e
    \ ld (hl),a
    \ _jp_next

  \ doc{
  \
  \ ctoggle ( b ca -- )
  \
  \ Invert the bits at _ca_ specified by the bitmask _b_.
  \
  \ See: `cset`, `creset`, `c@and`.
  \
  \ }doc

[unneeded] coff

?\ code coff ( ca -- ) E1 c, 36 c, 00 c, jpnext, end-code
  \ pop hl
  \ ld (hl),0
  \ _jp_next

  \ doc{
  \
  \ coff ( ca -- )
  \
  \ Store `false` at _ca_.
  \
  \ ``coff`` is written in Z80. Its equivalent definition in
  \ Forth is the following:

  \ ----
  \ : coff ( ca -- ) false swap c! ;
  \ ----

  \ See: `off`.
  \
  \ }doc

[unneeded] c? ?\ : c? ( ca -- ) c@ . ;

  \ doc{
  \
  \ c? ( ca -- )
  \
  \ Display the 1-byte unsigned integer stored at _ca_, using
  \ the format of `.`.
  \
  \ See: `?`, `c@`.
  \
  \ }doc

( !exchange c!exchange reserve alloted align aligned )

[unneeded] !exchange

?\ : !exchange ( x1 a -- x2 ) dup @ rot rot ! ;

  \ doc{
  \
  \ !exchange ( x1 a -- x2 )
  \
  \ Store _x1_ into _a_ and return its previous contents _x2_.
  \
  \ See: `c!exchange`, `exchange`.
  \
  \ }doc

[unneeded] c!exchange ?(

code c!exchange ( c1 ca -- c2 )
  D9 c, E1 c, C1 c, 7E c, 16 c, 00 c, 58 07 + c, D5 c,
  \ exx
  \ pop hl ; ca
  \ pop bc ; C = c1
  \ ld a,(hl)
  \ ld d,0
  \ ld e,a
  \ push de
  71 c, D9 c, jpnext, end-code ?)
  \ ld (hl),c
  \ exx
  \ _jp_next

  \ doc{
  \
  \ c!exchange ( c1 ca -- c2 )
  \
  \ Store _c1_ into _ca_ and return its previous contents _c2_.
  \
  \ ``c!exchange`` is written in Z80. An equivalent definition
  \ in Forth is the following:

  \ ----
  \ : c!exchange ( c1 ca -- c2 ) dup c@ rot rot c! ;
  \ ----

  \ See: `!exchange`, `cexchange`.
  \
  \ }doc

[unneeded] reserve

?\ : reserve ( n -- a ) here tuck over erase allot ;

  \ doc{
  \
  \ reserve ( n -- a )
  \
  \ Reserve _n_ address units of data space, erase the zone and
  \ return its address _a_.
  \
  \ }doc

[unneeded] alloted ?\ : allotted ( n -- a ) here swap allot ;

  \ doc{
  \
  \ allotted ( n -- a )
  \
  \ Reserve _n_ address units of data space and return its
  \ address _a_.
  \
  \ }doc

[unneeded] align ?\ need alias ' noop alias align immediate

  \ doc{
  \
  \ align ( -- )
  \
  \ If the data-space pointer is not aligned, reserve enough
  \ space to align it.
  \
  \ In Solo Forth, ``align`` does nothing (it's an `immediate`
  \ alias of `noop`), because the Z80 processor does not need
  \ addresses be cell-aligned.
  \
  \ Origin: Forth-94 (CORE), Forth-2012 (CORE).
  \
  \ See: `dp`, `aligned`.
  \
  \ }doc

[unneeded] aligned ?\ need alias ' noop alias aligned immediate

  \ doc{
  \
  \ aligned ( a1 -- a2 )
  \
  \ _a2_ is the first aligned address greater than or equal to
  \ _a1_.
  \
  \ In Solo Forth, ``aligned`` does nothing (it's an
  \ `immediate` alias of `noop`), because the Z80 processor
  \ does not need addresses be cell-aligned.
  \
  \ Origin: Forth-94 (CORE), Forth-2012 (CORE).
  \
  \ See: `align`.
  \
  \ }doc

( /! *! 2/! 2*! exchange cexchange )

[unneeded] /! ?\ : /! ( n a -- ) tuck @ swap / swap ! ;

  \ doc{
  \
  \ /! ( n a -- )
  \
  \ Divide _n_ by the single-cell number stored at _a_ and store
  \ the quotient in _a_
  \
  \ See: `2/!`, `*!`, `+!`, `-!`.
  \
  \ }doc

[unneeded] *! ?\ : *! ( n a -- ) tuck @ swap * swap ! ;

  \ doc{
  \
  \ *! ( n|u a -- )
  \
  \ Multiply _n|u_ by the single-cell number stored at _a_ and store
  \ the product in _a_
  \
  \ See: `2*!` `/!`, `+!`, `-!`.
  \
  \ }doc

[unneeded] 2*! ?\ : 2*! ( a -- ) dup @ 2* swap ! ;

  \ doc{
  \
  \ 2*! ( a -- )
  \
  \ Do a `2*` shift to the single-cell number stored at _a_.
  \
  \ See: `2/!`, `2*`.
  \
  \ }doc

[unneeded] 2/! ?\ need 2/ : 2/! ( a -- ) dup @ 2/ swap ! ;

  \ doc{
  \
  \ 2/! ( a -- )
  \
  \ Do a `2/` shift to the single-cell number stored at _a_.
  \
  \ See: `2*!`, `2/`.
  \
  \ }doc

[unneeded] exchange

?\ : exchange ( a1 a2 -- ) 2dup @ swap @  rot ! swap ! ;

  \ doc{
  \
  \ exchange ( a1 a2 -- )
  \
  \ Exchange the cells stored at _a1_ and _a2_.
  \
  \ See: `cexchange`, `!exchange`.
  \
  \ }doc

[unneeded] cexchange ?(

code cexchange ( ca1 ca2 -- )
  D9 c, E1 c, D1 c, 7E c, 47 c, 1A c, 77 c, 78 c, 12 c, D9 c,
  \ exx
  \ pop hl
  \ pop de
  \ ld a,(hl)
  \ ld b,a
  \ ld a,(de)
  \ ld (hl),a
  \ ld a,b
  \ ld (de),a
  \ exx
  jpnext, end-code ?)
  \ _jp_next

  \ doc{
  \
  \ cexchange ( ca1 ca2 -- )
  \
  \ Exchange the characters stored in _ca1_ and _ca2_.
  \
  \ ``cexchange`` is written in Z80. An equivalent definition
  \ in Forth is the following:
  \ 
  \ ----
  \ : cexchange ( ca1 ca2 -- ) 2dup c@ swap c@ rot c! swap c! ;
  \ ----

  \ See: `exchange`, `c!exchange`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015..2016: Main development.
  \
  \ 2016-04-17: Added `-!`. Documented some words.
  \
  \ 2016-04-23: Added `c-!`.
  \
  \ 2016-04-24: Remove `char`, which has been moved to the
  \ library.
  \
  \ 2016-04-25: Add `@cell+`. Move `n,`, `n@`, `n!` from the
  \ module "compilation.fsb".  Add `nn@`, `nn,`, `nn!`.
  \
  \ 2016-04-26: Remove unused words, specific of the TED
  \ editor.
  \
  \ 2016-04-27: Add `/!`, `*!`, `2/!`, `2*!`.
  \
  \ 2016-05-09: Add `align`, `aligned`.
  \
  \ 2016-05-10: Remove the dependency on the assembler.
  \ Compact the blocks. Remove unfinished words from cmForth.
  \ Rename `@cell+` to `@+`. Add `2@+`.
  \
  \ 2016-08-01: Fix header line.
  \
  \ 2016-08-02: Fix requiring `c1+!`, `c1-!`, `1+!` and `1-!`.
  \
  \ 2016-11-16: Fix documentation.
  \
  \ 2016-11-22: Document `align` and `aligned`.
  \
  \ 2016-11-23: Rename `c!toggle-bits` to `ctoggle`,
  \ `c!set-bits` to `cset`, `c!reset-bits` to `creset`, and
  \ `c@test-bits` to `c@and` after the changes in the system.
  \
  \ 2016-11-26: Improve `2@+`.
  \
  \ 2016-12-08: Rename the module filename with uppercase
  \ "MISC" after the new convention. Rename `exchange` to
  \ `!exchange`. Add `c!exchange`. Add new `exchange` and
  \ `cexchange`.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-03-21: Improve documentation. Convert `c@+` to an
  \ alias of `count`.
  \
  \ 2017-04-09: Fix needing of `bit>mask` and family.
  \
  \ 2017-05-07: Fix markup in documentation.
  \
  \ 2017-05-10: Improve documentation. Rewrite `cexchange` and
  \ `c!exchange` in Z80.
  \
  \ 2017-12-12: Add `coff`.

  \ vim: filetype=soloforth
