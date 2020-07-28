  \ memory.far.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202007282031
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to the far-memory system, a virtual 64 KiB
  \ space formed by 4 configurable 16 KiB banks of paged
  \ memory. The base support and the main words to use far
  \ memory are included in the kernel (see `far-banks` to
  \ start).

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2016, 2017, 2018, 2019,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ ===========================================================
  \ Credit

  \ The "far" naming convention was borrowed from a post by Dr
  \ Jefyll in 6502.org on 2010-02-16:
  \
  \ http://forum.6502.org/viewtopic.php?f=9&t=1529&p=10079
  \
  \ Also Garry Lancaster used the name "far memory" in his Z88
  \ CamelForth (2001):
  \
  \ http://www.worldofspectrum.org/z88forever/camelforth/camel-pools.html

( far-hl_ ?next-bank_ ?previous-bank_ np! )

get-current assembler-wordlist dup >order set-current

unneeding far-hl_ ?\ ' far 2+ @ constant far-hl_

  \ doc{
  \
  \ far-hl_ ( -- a ) "far-h-l-underscore"
  \
  \ Address of the ``far.hl`` routine of the kernel, which
  \ converts the far-memory address ($0000..$FFFF) hold in the
  \ HL register to its actual equivalent ($C000..$FFFF) and
  \ page in the corresponding memory bank.
  \
  \ This is the routine called by `far`. ``far-hl_`` is used in
  \ `code` words.
  \

  \ Input:
  \
  \ - HL = far-memory address ($0000..$FFFF)
  \
  \ Output:
  \
  \ - HL = actual memory address ($C000..$FFFF)
  \ - A DE corrupted

  \ }doc

unneeding ?next-bank_

?\ ' ?next-bank 2+ @ constant ?next-bank_

  \ doc{
  \
  \ ?next-bank_ ( -- a ) "question-next-bank-underscore"
  \
  \ Address of the ``question_next_bank`` routine of the
  \ kernel, which does the following:
  \
  \ If the actual far-memory address ($C000..$FFFF) in the HL
  \ register has increased to the next bank ($0000..$3FFF),
  \ convert it to the corresponding actual address
  \ ($C000..$FFFF) in the next bank and page in the next bank,
  \ else do nothing.
  \
  \ This is the routine called by `?next-bank`.  `?next-bank_`
  \ is used in `code` words.
  \

  \ Input:
  \
  \ - HL = address in a paged bank ($C000..$FFFF) or higher
  \   ($0000..$BFFF).
  \
  \ Output when HL is above the paged bank:
  \
  \ - HL = corresponding address in the next bank, which is paged in
  \ - A corrupted
  \ - D = 0
  \ - E = bank
  \
  \ Output when HL is an address in a paged bank:
  \
  \ - HL preserved
  \ - A corrupted

  \ }doc

unneeding ?previous-bank_ ?(

' ?previous-bank 2+ @ constant ?previous-bank_

?)

  \ doc{
  \
  \ ?previous-bank_ ( -- a ) "question-previous-bank-underscore"
  \
  \ Address of the ``question_previous_bank`` routine of the
  \ kernel, which does the followig:
  \
  \ If the actual far-memory address ($C000..$FFFF) in the HL
  \ register has decreased to the previous bank ($8000..$BFFF),
  \ convert it to the corresponding actual address
  \ ($C000..$FFFF) in the previous bank and page in the next
  \ bank, else do nothing.
  \
  \ This is the routine called by `?previous-bank`.
  \ `?previous-bank_` is used in `code` words.
  \

  \ Input:
  \
  \ - HL = address in a paged bank ($C000..$FFFF) or lower
  \   ($8000..$BFFF).
  \
  \ Output when HL is below the paged bank:
  \
  \ - HL = corresponding address in the previous bank, which is paged in
  \ - A corrupted
  \ - D = 0
  \ - E = bank
  \
  \ Output when HL is an address in a paged bank:
  \
  \ - HL preserved
  \ - A corrupted

  \ }doc

previous set-current

unneeding np!

?\ code np! ( x -- ) E1 c, 22 c, np , jpnext, end-code

  \ pop hl
  \ ld (names_pointer),hl
  \ _jp_next

  \ doc{
  \
  \ np! ( a -- ) "n-p-store"
  \
  \ Store _a_ into the name-space pointer `np`.
  \
  \ ``np!`` is written in Z80. Its equivalent definition in
  \ Forth is the following:

  \ ----
  \ : np! ( a -- ) np ! ;
  \ ----

  \ }doc

( default-bank_ e-bank_ )

get-current assembler-wordlist dup >order set-current

unneeding default-bank_

?\ ' default-bank 2+ constant default-bank_

  \ doc{
  \
  \ default-bank_ ( -- a ) "default-bank-underscore"
  \
  \ Return address _a_ of a routine that pages in the default
  \ bank. This is the routine `default-bank` runs into, after
  \ pushing IX on the return stack to force a final return to
  \ `next`.
  \
  \ Output of the routine: A and E corrupted.
  \
  \ See also: `e-bank_`.
  \
  \ }doc

unneeding e-bank_

?\ ' default-bank 4 + constant e-bank_

  \ doc{
  \
  \ e-bank_ ( -- a ) "e-bank-underscore"
  \
  \ Return address _a_ of a routine that pages in the
  \ bank hold in the E register.
  \ This routine is a secondary entry point of `default-bank`.
  \
  \ - Input: E = bank
  \ - Output: A corrupted
  \
  \ See also: `default-bank_`.
  \
  \ }doc

previous set-current

( farallot far, far-n, farfill farerase )

unneeding farallot ?\ : farallot ( n -- ) np +! ;

  \ doc{
  \
  \ farallot ( n -- ) "far-allot"
  \
  \ If _n_ is greater than zero, reserve _n_ bytes of headers
  \ space. If _n_ is less than zero, release _n_ bytes of
  \ headers space. If _n_ is zero, leave the headers-space
  \ pointer unchanged.
  \
  \ See also: `farfill`, `far-banks`.
  \
  \ }doc

unneeding far, ?( need farallot

: far, ( x -- ) np@ far! cell farallot ; ?)

  \ doc{
  \
  \ far, ( x -- ) "far-comma"
  \
  \ Compile _x_ in far-memory headers space.
  \
  \ See also: `far-n,`, `,`, `farallot`.
  \
  \ }doc

unneeding far-n, ?( need far,

: far-n, ( x[u]..x[1] u -- ) 0 ?do far, loop ; ?)

  \ doc{
  \
  \ far-n, ( x[u]..x[1] u -- ) "far-n-comma"
  \
  \ If _u_ is not zero, store _u_ cells _x[u]..x[1]_ into
  \ far-memory headers
  \ space, being _x[1]_ the first one stored and _x[u]_ the
  \ last one.
  \
  \ See also: `far,`, `n,`, `farallot`.
  \
  \ }doc

unneeding farfill ?(

: farfill ( ca len b -- )
  rot rot bounds ?do dup i farc! loop drop ; ?)

  \ doc{
  \
  \ farfill ( ca len b -- ) "far-fill"
  \
  \ If _len_ is not zero, store _b_ in each of _len_
  \ consecutive characters of `far` memory beginning at _a_.
  \
  \ See also: `farerase`, `farallot`, `far-n,`, `farc!`,
  \ `far-banks`, `fill`.
  \
  \ }doc

unneeding farerase

?\ : farerase ( ca len -- ) bounds ?do 0 i farc! loop ;

  \ doc{
  \
  \ farerase ( ca len -- ) "far-erase"
  \
  \ If _len_ is greater than zero, clear all bits in each of
  \ _len_ consecutive address units of `far` memory beginning at
  \ _ca_.
  \
  \ See also: `farfill`, `farallot`, `far-n,`, `farc!`, `far-banks`.
  \
  \ }doc

( far2@ far2! far@+ farc@+ far+! farc+! far2@+ )

unneeding far2@
?\ : far2@ ( a -- d ) dup cell+ far@ swap far@ ;

  \ doc{
  \
  \ far2@ ( a -- d ) "far-two-fetch"
  \
  \ Fetch _d_ from far-memory address _a_.
  \
  \ See also: `far2!`, `far2@+`, `far@`, `farc@`, `far-banks`, `2@`.
  \
  \ }doc

unneeding far2!
?\ : far2! ( d a -- ) swap over far! cell+ far! ;

  \ doc{
  \
  \ far2! ( d a -- ) "far-two-store"
  \
  \ Store _d_ into far-memory address _a_.
  \
  \ See also: `far2@`, `far!`, `farc!`, `far-banks`, `2!`.
  \
  \ }doc

unneeding far@+
?\ : far@+ ( a -- a' x ) dup cell+ swap far@ ;

  \ doc{
  \
  \ far@+ ( a -- a' x ) "far-fetch-plus"
  \
  \ Fetch _x_ from far-memory address _a_. Return _a'_, which
  \ is _a_ incremented by one cell.  This is handy for stepping
  \ through cell arrays.
  \
  \ See also: `farc@+`, `far@+`, `far2@+`, `@+`, `far-banks`.
  \
  \ }doc

unneeding farc@+
?\ : farc@+ ( ca -- ca' c ) dup char+ swap farc@ ;

  \ doc{
  \
  \ farc@+ ( ca -- ca' c ) "far-c-fetch-plus"
  \
  \ Fetch the character _c_ at far-memory address _ca_. Return
  \ _ca'_, which is _ca_ incremented by one character.  This
  \ is handy for stepping through character arrays.
  \
  \ See also: `far@+`, `far-banks`.
  \
  \ }doc

unneeding far+!
?\ : far+! ( n a -- ) dup far@ rot + swap far! ;

  \ doc{
  \
  \ far+! ( n|u a -- ) "far-plus-store"
  \
  \ Add _n|u_ to the single-cell number at far-memory address
  \ _a_.
  \
  \ See also: `farc+!`, `+!`, `farc!`, `far-banks`.
  \
  \ }doc

unneeding farc+!
?\ : farc+! ( c a -- ) dup farc@ rot + swap farc! ;

  \ doc{
  \
  \ farc+! ( c ca - ) "far-c-plus-store"
  \
  \ Add _c_ to the char at far-memory address _ca_.
  \
  \ See also: `far+!`, `c+!`, `farc!`, `far-banks`.
  \
  \ }doc

unneeding 2@+ ?exit need far2@
: far2@+ ( a -- a' xd ) dup cell+ cell+ swap far2@ ;

  \ doc{
  \
  \ far2@+ ( a -- a' xd ) "far-two-fetch-plus"
  \
  \ Fetch _xd_ from _a_. Return _a'_, which is _a_ incremented
  \ by two cells.  This is handy for stepping through
  \ double-cell arrays.
  \
  \ See also: `far@+`, `farc@+`, `far2@`, `2@+`. `far-banks`.
  \
  \ }doc

( move>far move<far cmove>far cmove<far )

unneeding move>far ?( need +loop

: move>far ( a1 a2 len -- )
  cells bounds ?do  dup @ i far! cell+ cell +loop drop ; ?)

  \ doc{
  \
  \ move>far ( a1 a2 len -- ) "move-to-far"
  \
  \ If _len_ is greater than zero, copy _len_ consecutive
  \ cells from main-memory address _a1_ to far-memory
  \ address _a2_.
  \
  \ }doc

unneeding move<far ?( need +loop

: move<far ( a1 a2 len -- )
  cells bounds ?do  dup far@ i ! cell+ cell +loop  drop ; ?)

  \ doc{
  \
  \ move<far ( a1 a2 len -- ) "move-from-far"
  \
  \ If _len_ is greater than zero, copy _len_ consecutive
  \ cells from far-memory address _a1_ to main-memory
  \ address _a2_.
  \
  \ }doc

unneeding cmove>far ?(

: cmove>far ( ca1 ca2 len -- )
  bounds ?do  dup c@ i farc! char+  loop  drop ; ?)

  \ doc{
  \
  \ cmove>far ( ca1 ca2 len -- ) "c-move-to-far"
  \
  \ If _len_ is greater than zero, copy _len_ consecutive
  \ characters from main-memory address _ca1_ to far-memory
  \ address _ca2_.
  \
  \ }doc

unneeding cmove<far ?(

: cmove<far ( ca1 ca2 len -- )
  bounds ?do  dup farc@ i c! char+  loop  drop ; ?)

  \ doc{
  \
  \ cmove<far ( ca1 ca2 len -- ) "c-move-from-far"
  \
  \ If _len_ is greater than zero, copy _len_ consecutive
  \ characters from far-memory address _ca1_ to main-memory
  \ address _ca2_.
  \
  \ }doc

( !bank c!bank @bank c@bank /bank bank-start )

unneeding !bank ?( need e-bank_

code !bank ( x a n -- ) D1 c, e-bank_ call, E1 c, D1 c, 73 c,
  23 c, 72 c, ' default-bank jp, end-code ?)
  \ pop de
  \ call bank.e
  \ pop hl
  \ pop de
  \ ld (hl),e
  \ inc hl
  \ ld (hl),d
  \ jp default_bank_

  \ doc{
  \
  \ !bank ( x a n -- ) "store-bank"
  \
  \ Store cell _x_ into address _a_ ($C000..$FFFF) of `bank`
  \ _n_.
  \
  \ ``!bank`` is written in Z80. Its equivalent definition in
  \ Forth is the following:

  \ ----
  \ : !bank ( x a n -- ) bank ! default-bank ;
  \ ----

  \ See also: `@bank`, `c!bank`.
  \
  \ }doc

unneeding !bank ?( need e-bank_

code c!bank ( c ca n -- ) D1 c, e-bank_ call,
  E1 c, D1 c, 73 c, ' default-bank jp, end-code ?)
  \ pop de
  \ call bank.e
  \ pop hl
  \ pop de
  \ ld (hl),e
  \ jp default_bank_

  \ doc{
  \
  \ c!bank ( c ca n -- ) "c-store-bank"
  \
  \ Store _c_ into address _ca_ ($C000..$FFFF) of `bank` _n_.
  \
  \ ``c!bank`` is written in Z80. Its equivalent definition in
  \ Forth is the following:

  \ ----
  \ : c!bank ( c ca n -- ) bank c! default-bank ;
  \ ----

  \ See also: `c@bank`, `!bank`.
  \
  \ }doc

unneeding @bank ?( need e-bank_

code @bank ( a n -- x )
  D1 c, e-bank_ call, E1 c, 7E c, 23 c, 66 c, 6F c,
  \ pop de
  \ call bank.e
  \ pop hl
  \ ld a,(hl)
  \ inc hl
  \ ld h,(hl)
  \ ld l,a
  E5 c, ' default-bank jp, end-code ?)
  \ push hl
  \ jp default_bank_

  \ doc{
  \
  \ @bank ( a n -- x ) "fetch-bank"
  \
  \ Fetch _x_ from address _a_ ($C000..$FFFF) of `bank` _n_.
  \
  \ ``@bank`` is written in Z80. Its equivalent definition in
  \ Forth is the following:

  \ ----
  \ : @bank ( a n -- x ) bank @ default-bank ;
  \ ----

  \ See also: `!bank`, `c@bank`.
  \
  \ }doc

unneeding c@bank ?( need e-bank_

code c@bank ( ca n -- c ) D1 c, e-bank_ call,
  E1 c, 6E c, 26 c, 00 c, E5 c, ' default-bank jp, end-code ?)
  \ pop de
  \ call bank.e
  \ pop hl
  \ ld l,(hl)
  \ ld h,0
  \ push hl
  \ jp default_bank_

  \ doc{
  \
  \ c@bank ( ca n -- c ) "c-fetch-bank"
  \
  \ Fetch _c_ from address _ca_ ($C000..$FFFF) of `bank` _n_.
  \
  \ ``c@bank`` is written in Z80. Its equivalent definition in
  \ Forth is the following:

  \ ----
  \ : c@bank ( ca n -- c )
  \   bank c@ default-bank ;
  \ ----

  \ See also: `c!bank`, `@bank`.
  \
  \ }doc

unneeding /bank ?\ $4000 constant /bank

  \ doc{
  \
  \ /bank ( -- n ) "slash-bank"
  \
  \ _n_ is the size in bytes of a memory bank: $4000.
  \
  \ See also: `bank-start`.
  \
  \ }doc

unneeding bank-start ?\ $C000 constant bank-start

  \ doc{
  \
  \ bank-start ( -- a )
  \
  \ _a_ is the memory address where banks are paged in: $C000.
  \
  \ See also: `/bank`, `bank`, `banks`, `far-banks`,
  \ `default-bank`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-11-15: Start.  Add `farhl`, `questionnextbank`,
  \ `questionpreviousbank`.
  \
  \ 2016-11-16: Add `far2@`, `far@+`, `farc@+`, `far2@+`,
  \ `far+!`, `farc+!`, `far2!` `move>far`, `move<far`,
  \ `cmove>far`, `cmove<far`, `fartype-ascii`, `farallot`.
  \
  \ 2016-11-18: Credit about the naming convention.
  \
  \ 2016-11-26: Improve `far2@+`. Move `fartype-ascii` to the
  \ printing module.
  \
  \ 2016-12-24: Fix block header. Improve the Asciidoctor
  \ markup of the documentation.
  \
  \ 2016-12-30: Compact the code, saving one block.
  \
  \ 2017-01-05: Update `also assembler` to `assembler-wordlist
  \ >order`.
  \
  \ 2017-01-09: Add `far,`.
  \
  \ 2017-01-10: Change `2*` to `cells`.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation.
  \
  \ 2017-02-01: Fix typo. Improve the layout of some
  \ documentation.  Add `default-bank-routine`,
  \ `e-bank-routine`.  Move `!bank`, `c!bank`, `@bank` and
  \ `c@bank` from the kernel. Rename `farhl` to
  \ `far-hl-routine`; `questionnextbank` to
  \ `?next-bank-routine`; `questionpreviousbank` to
  \ `?previous-bank-routine`.
  \
  \ 2017-02-16: Fix typo in documentation of `farc@+`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-26: Update "hp" notation to "np", after the changes
  \ in the kernel.
  \
  \ 2017-03-04: Update naming convention of Z80 routines, after
  \ the changes in the kernel.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-05-10: Improve documentation.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-07: Add words' pronunciaton.
  \
  \ 2018-03-09: Update notation "address units" to "bytes".
  \
  \ 2018-04-12: Fix markup in documentation.
  \
  \ 2019-03-15: Add `far-n,`.
  \
  \ 2019-03-21: Add `farfill` and `farerase`. Improve
  \ documentation.
  \
  \ 2020-05-04: Fix cross references.
  \
  \ 2020-05-18: Update: `+loop` was moved to the library. Move
  \ `np!` here from the kernel.
  \
  \ 2020-05-19: Improve documentation.
  \
  \ 2020-06-09: Move `/bank` and `bank-start` from the
  \ <memory.bank.fs> module.
  \
  \ 2020-06-15: Improve documentation.
  \
  \ 2020-06-16: Fix cross-reference.

  \ vim: filetype=soloforth
