  \ memory.address_register.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201806041324
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Address register store and fetch words.
  \
  \ There aren't any spare registers to make this as efficient
  \ as it could be. However, it can still give a useful
  \ improvement in loops, and in many cases also results in
  \ cleaner-looking code.  (From the original source of Z88
  \ CamelForth, by Garry Lancaster.)

  \ ===========================================================
  \ Authors

  \ Garry Lancaster wrote the original code for Z88 CamelForth,
  \ 2001.
  \
  \ Marcos Cruz (programandala.net) adapted the code for Solo
  \ Forth, 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( a a! a@ !a @a c!a c@a )

unneeding a ?( variable a

  \ doc{
  \
  \ a ( -- a )
  \
  \ A `variable`. _a_ is the address of a cell containing the
  \ address register.
  \
  \ See: `a!`, `a@`, `!a`, `@a`, `c!a`, `c@a`, `!a+`, `@a+`,
  \ `c!a+`, `c@a+`.
  \
  \ }doc

code a! ( a -- ) E1 c, 22 c, a , jpnext, end-code
    \ pop hl
    \ ld (address_register),hl
    \ _jp_next

  \ doc{
  \
  \ a! ( a -- ) "a-store"
  \
  \ Set the address register.
  \
  \ See: `a`, `a@`.
  \
  \ }doc

code a@ ( -- a ) 2A c, a , E5 c, jpnext, end-code ?)
    \ ld hl,(address_register)
    \ push hl
    \ _jp_next

  \ doc{
  \
  \ a@ ( -- a ) "a-fetch"
  \
  \ Get the address register.
  \
  \ See: `a`, `a!`.
  \
  \ }doc

unneeding !a ?( need a

code !a ( x -- ) D1 c, 2A c, a , 70 03 + c, 23 c, 70 02 + c,
                 jpnext, end-code ?)
    \ pop de
    \ ld hl,(address_register)
    \ ld (hl),e
    \ inc hl
    \ ld (hl),d
    \ _jp_next

  \ doc{
  \
  \ !a ( x -- ) "store-a"
  \
  \ Store _x_ at the address register.
  \
  \ See: `a`, `@a`.
  \
  \ }doc

unneeding @a ?( need a

code @a ( -- x ) 2A c, a , 5E c, 23 c, 66 c, 68 03 + c, E5 c,
                 jpnext, end-code ?)
    \ ld hl,(address_register)
    \ ld e,(hl)
    \ inc hl
    \ ld h,(hl)
    \ ld l,e
    \ push hl
    \ _jp_next

  \ doc{
  \
  \ @a ( -- x ) "fetch-a"
  \
  \ Fetch _x_ at the address register.
  \
  \ See: `a`, `!a`.
  \
  \ }doc

unneeding c!a ?( need a

code c!a ( c -- ) D1 c, 2A c, a , 70 03 + c, jpnext,
                  end-code ?)
    \ pop de
    \ ld hl,(address_register)
    \ ld (hl),e
    \ _jp_next

  \ doc{
  \
  \ c!a ( c -- ) "c-fetch-a"
  \
  \ Store _c_ at the address register.
  \
  \ See: `a`, `c@a`.
  \
  \ }doc

unneeding c@a ?(

code c@a ( -- c ) 2A c, a , 6E c, 26 c, 00 c, E5 c, jpnext,
                  end-code ?)
    \ ld hl,(address_register)
    \ ld l,(hl)
    \ ld h,0
    \ push hl
    \ _jp_next

  \ doc{
  \
  \ c@a ( -- c ) "c-fetch-a"
  \
  \ Fetch _c_ at the address register.
  \
  \ See: `a`, `c!a`.
  \
  \ }doc

( !a+ @a+ c!a+ c@a+ )

unneeding !a+ ?( need a

code !a+ ( x -- ) D1 c, 2A c, a , 70 03 + c, 23 c, 70 02 + c,
                  23 c, 22 c, a , jpnext, end-code ?)
    \ pop de
    \ ld hl,(address_register)
    \ ld (hl),e
    \ inc hl
    \ ld (hl),d
    \ inc hl
    \ ld (address_register),hl
    \ _jp_next

  \ doc{
  \
  \ !a+ ( x -- ) "store-a-plus"
  \
  \ Store _x_ at the address register and increment the address
  \ register by one cell.
  \
  \ See: `a`, `@a+`.
  \
  \ }doc

unneeding @a+ ?( need a

code @a+ ( -- x ) 2A c, a , 5E c, 23 c, 56 c, 23 c, 22 c, a ,
                  D5 c, jpnext, end-code ?)
    \ ld hl,(address_register)
    \ ld e,(hl)
    \ inc hl
    \ ld d,(hl)
    \ inc hl
    \ ld (address_register),hl
    \ push de
    \ _jp_next

  \ doc{
  \
  \ @a+ ( -- x ) "fetch-a-plus"
  \
  \ Fetch cell at the address register and increment the
  \ address register by one cell.
  \
  \ See: `a`, `!a+`.
  \
  \ }doc


unneeding c!a+ ?( need a

code c!a+ ( c -- ) D1 c, 2A c, a , 70 03 + c, 23 c,
                   22 c, a , jpnext, end-code ?)
    \ pop de
    \ ld hl,(address_register)
    \ ld (hl),e
    \ inc hl
    \ ld (address_register),hl
    \ _jp_next

  \ doc{
  \
  \ c!a+ ( c -- ) "c-store-a-plus"
  \
  \ Store _c_ at the address register and increment the address
  \ register by one address unit.
  \
  \ See: `a`, `c@a+`.
  \
  \ }doc

unneeding c@a+ ?( need a

code c@a+ ( -- c ) 2A c, a , 5E c, 23 c, 16 c, 00 c,
                   22 c, a , D5 c, jpnext, end-code ?)
    \ ld hl,(address_register)
    \ ld e,(hl)
    \ inc hl
    \ ld d,0
    \ ld (address_register),hl
    \ push de
    \ _jp_next

  \ doc{
  \
  \ c@a+ ( -- c ) "c-fetch-a-plus"
  \
  \ Fetch _c_ at the address register and increment the address
  \ register by one address unit.
  \
  \ See: `a`, `c!a+`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-07-20: Copy the code from Z88 CamelForth. Start
  \ adapting it.
  \
  \ 2015-07-21: Finish the adaption. Not tested yet.
  \
  \ 2015-07-22: Let independent loading of the words.
  \
  \ 2016-05-07: Make block titles compatible with `indexer`.
  \
  \ 2016-05-10: Compact the blocks. Fix `!a`, `!a+`, `c@a+`.
  \ Document.
  \
  \ 2016-11-13: Use `jppushhl` instead of `jp pushhl`; improve
  \ comments.
  \
  \ 2016-12-20: Rename `jppushhl` to `jppushhl,` and `jpnext`
  \ to `jpnext,`, after the change in the kernel.
  \
  \ 2017-02-23: Fix notation of cross references in
  \ documentation.
  \
  \ 2017-05-09: Remove `jppushhl,`.
  \
  \ 2017-07-27: Replace `jp next` with the actual macro
  \ `_jp_next` in Z80 comments.
  \
  \ 2017-12-09: Improve documentation.
  \
  \ 2018-02-14: Compact the code, saving one block.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-07: Add words' pronunciaton.
  \
  \ 2018-06-04:  Link `variable` in documentation.

  \ vim: filetype=soloforth
