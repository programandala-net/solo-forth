  \ meta.test.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201702282329

  \ -----------------------------------------------------------
  \ Description

  \ Development tests.

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

  \ 2016-04-24: Remove `[char]` and `char`, which have been
  \ moved to the library.
  \
  \ 2016-04-28: Move the tests of `type-center`, `type-left`
  \ and `type-right` from the module "printing.type.fsb".  Add
  \ `?ccase-test`, `ccase0-test`. Add `jk-test`.
  \
  \ 2016-05-01: Add `color-test`.
  \
  \ 2016-05-05: Add `search-test`.
  \
  \ 2016-05-07: Add `need-test`, `?(-test`.
  \
  \ 2016-05-17: Need `>body`, which has been moved to the
  \ library.
  \
  \ 2016-08-05: Move `begincase-test` from
  \ <lib/flow.begincase.fsb>.
  \
  \ 2016-10-26: Add `a>e-test`.
  \
  \ 2016-11-13: Update the names of far-memory words.
  \
  \ 2016-11-25: Move `:switch-test` and `[switch-test` from
  \ <flow.bracket-switch.fsb>. Add tests for `[:`.
  \
  \ 2016-11-26: Need `catch`, which has been moved to the
  \ library.
  \
  \ 2016-11-27: Move `doer-test` from the `doer` module.
  \
  \ 2016-12-03: Add `nest-need-test`.
  \
  \ 2016-12-07: Add `transient-test`.
  \
  \ 2016-12-07: Add `transient-asm-test`.
  \
  \ 2016-12-08: Rename the module filename with uppercase
  \ "MISC" after the new convention.
  \
  \ 2016-12-15: Move `associative-list-test` from the
  \ `associative-list` module.
  \
  \ 2016-12-16: Add `sconstants-test` and `/sconstants-test`.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2016-12-24: Add `window-test`.
  \
  \ 2016-12-29: Move `transient-test` from block 1.
  \
  \ 2017-01-02: Use `z80-asm,` in `transient-asm-test`.
  \
  \ 2017-01-05: Remove the tests of the old `z80-asm`
  \ assembler.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-01-06: Update `voc-link` to `latest-wordlist`.
  \
  \ 2017-01-20: Add `>name-test`.
  \
  \ 2017-01-22: Add `substitute-test`.
  \
  \ 2017-01-23: Move `select-test` from <flow.select.fsb>; move
  \ `fzx-text` from <screen_mode.fzx.fsb>; move
  \ `edit-sound-test` from <sound.128.editor.fsb>.
  \
  \ 2017-02-01: Replace `upper` with `lower`, because `upper`
  \ has been moved to the library.
  \
  \ 2017-02-08: Update the usage of `set-drive`, which now
  \ returns an error result. Move `.tape` from the tape module.
  \
  \ 2017-02-17: Update notation "behaviour" to "action".
  \
  \ 2017-02-19: Add `-do-test`.  Replace `do`, which has been
  \ moved to the library, with `?do`.
  \
  \ 2017-02-21: Add `bank-test`.
  \
  \ 2017-02-24: Add `udg-row[-test`.
  \
  \ 2017-02-26: Update "hp" notation to "np", after the changes
  \ in the kernel.
  \
  \ 2017-02-28: Add `zx7s-test`.

( zx7s-test )

page
  .( zx7s-test loading) cr
need zx7s  need file>
create compresssed 6912 allot

  .( Press any key to load the) cr
  .( compressed screen from the) cr
  .( first disk drive.) key drop
s" img.zx7"  compressed 0 file>

  .( Press any key to decompress and) cr
  .( display the screen.) key drop
compressed 16384 zx7s

( udg-row[-test )

need udg-row[ need set-udg

create graphic 16 allot  graphic set-udg

  0 udg-row[

  000000000010010000000000
  000000000010010000000000
  000000000110011000000000
  001111111111111111111100
  011111111111111111111110
  111111111111111111111111
  111111111111111111111111
  011111111111111111111110
  ]udg-row

  cr 0 emit-udg 1 emit-udg 2 emit-udg cr

( bank-test )

$FFFF constant test-addr

: b? ( n -- ) bank test-addr c@ . ;
  \ Show the test value stored in bank _n_, which must be _n_.

: (prepare-bank) ( n -- )
  dup . dup bank banks 1- and test-addr c! ;
  \ Prepare bank _n_, saving the test value to it.

: prepare-bank ( n -- )
  dup 5 = if drop else (prepare-bank) then ;
  \ If _n_ is not 5, prepare bank _n_, saving the test value to
  \ it.  Bank 5 is ommited because it is paged also at $4000.

: prepare-banks ( n1 n2 -- )
  cr ." Preparing banks:" ?do i prepare-bank loop ;
  \ Prepare all banks from _n2_ to _n1-1_, saving the test
  \ value to them.

: check-bank ( n -- ) dup . dup bank test-addr c@ =
                      if ." OK" else ." NO" then space ;
  \ Check if bank _n_ contains the test value.

: check-banks ( n1 n2 -- )
  cr ." Checking banks:" ?do i check-bank loop ;
  \ Check if all banks from _n2_ to _n1-1_ contain the test
  \ value.

: run ( n1 n2 -- )
  2dup prepare-banks check-banks default-bank ;

( -do-test )

need do need -do

cr .( Results of '-1 +loop' loops:)

: count-down ( limit start -- ) do i . -1 +loop ;

cr .( '0 0 do' prints 0) 0 0 count-down
cr .( '4 0 do' prints 0 -1..-32768 32767..4) \ 4 0 count-down
cr .( '0 4 do' prints 4 3 2 1 0:) 0 4 count-down

: ?count-down ( limit start -- ) ?do i . -1 +loop ;

cr .( '0 0 ?do' prints nothing:) 0 0 ?count-down
cr .( '4 0 ?do' prints 0 -1..-32768 32767..4) \ 4 0 ?count-down
cr .( '0 4 ?do' prints 4 3 2 1 0:) 0 4 ?count-down

: -count-down ( limit start -- ) -do i . -1 +loop ;

cr .( '0 0 -do' prints nothing:) 0 0 -count-down
cr .( '4 0 -do' prints nothing:) 4 0 -count-down
cr .( '0 4 -do' prints 4 3 2 1:) 0 4 -count-down

( .tape )

: .tape ( -- )
  cr ." Tape header " tape-header u. cr
     ." Filetype    " tape-filetype c@ . cr
     ." Filename    " tape-filename /tape-filename type cr
     ." Length      " tape-length @ u. cr
     ." Start       " tape-start @ u. cr
     .s cr
     ." Press any key" key drop ;

( edit-sound-test )

  \ XXX TMP

need edit-sound need >body need shoot

create explosion /sound allot

' shoot >body explosion /sound move

explosion edit-sound

( fzx-test )

  \ XXX TMP for debugging

need fzx-mode need <file

create font  2048 allot

  .( Loading a FZX font...)
1 set-drive throw  font 0 s" lett.fzx" <file
font fzx-font !

: zxtype ( ca len -- ) bounds ?do  i c@ fzx-emit  loop ;

cr .( fzx-emit is ready ) cr
' (fzx-emit)
cr .( Code start:   ) dup u.
cr .( Code length:  ) ' fzx-emit swap - u.
cr

( select-test )

  \ Usage example

: select-test ( n -- )
  space
  select
    cond  $00 $1F range
          $7F     equal  when  ." Control char "       else
    cond  $20 $2F range
          $3A $40 range
          $5B $60 range
          $7B $7E range  when  ." Point "              else
    cond  $30 $39 range  when  ." Digit "              else
    cond  $41 $5A range  when  ." Upper case letter "  else
    cond  $61 $7A range  when  ." Lower case letter "  else
    ." Not a character "
  endselect ;  -->

( select-test )

cr cr .( Running 'select' test...)

cr  'a'  .(   ) dup emit  select-test
cr  ','  .(   ) dup emit  select-test
cr  '8'  .(   ) dup emit  select-test
cr  '?'  .(   ) dup emit  select-test
cr  'K'  .(   ) dup emit  select-test
cr  0              dup 3 .r  select-test
cr  127            dup 3 .r  select-test
cr  128            dup 3 .r  select-test

( substitute-test )

need substitute need replaces need xt-replaces

create result 255 chars allot

s" saluton"  s" hello"    replaces
s" bonvenon" s" welcome"  replaces

: substitute-test ( ca len -- )
  2dup cr type result 255 substitute cr dup 0< over ?throw
  . ." substitutions:" cr type cr ;

s" Say '%hello%' and '%welcome%'" substitute-test
s" The percentage sign is %%" substitute-test

: fth  s" Forth" ; ' fth s" lang" xt-replaces
s" I program in %lang%" substitute-test

: z80  s" Z80 assembly" ; ' z80 s" lang" xt-replaces
s" I program in %lang%" substitute-test

s" Say '%hello%' in %lang%" substitute-test

( >name-test )

need far,

variable name0  1000 far,
variable name1

  \ This works fine, because `defined` searches the dictionary
  \ in the usual direction (from newest to oldest):

defined name0 cr .name
defined name1 cr .name

  \ On the contrary, `>name` searches the dictionary in reverse
  \ order (from oldest to newest).  This works fine too:

' name0 >name cr .name

  \ But this fails, because the 1000 is compiled in header
  \ space, between the definition of `name0` and that of
  \ `name1`, and the calculation to get the header of `name1`
  \ from `name0` depends on the assumption that there's nothing
  \ between both headers:

' name1 >name dup 0=
 ?\ .( not found!)  \ or simply hang!
 ?\ .name

( transient-test )

  \ XXX FIXME --
  \ 2016-12-29: The headers space reserved for the assembler
  \ must be 246 bytes more than the actual space used. Why?

  need fyi need transient
  \ 1900 1700 transient  \ fails
  \ 2000 2000 transient  \ fails
  \ 2008 2000 transient  \ fails
  \ 2016 2000 transient  \ fails
  \ 2020 2000 transient  \ fails
  \ 2022 2000 transient  \ fails
    2023 1700 transient  \ works
  \ 2023 2000 transient  \ works
  \ 2025 2000 transient  \ works
  \ 2050 2000 transient  \ works
  \ 2100 2000 transient  \ works
  \ 2250 2000 transient  \ works
  \ 2500 2000 transient  \ works
  unused farunused
  need assembler   cr .( Memory used by the assembler:)
      farunused - cr u. .(  headers space)    \ 1777 B
         unused - cr u. .(  data/code space)  \ 1623 B
  cr .( Is headers space corrupted?:)
  cr .( This should read "26972": ) 0 far@ dup u.
  26972 = ?\ cr .( BUT IT DOESN'T!)
  cr .( This name:   ") $04 .name .( ")
  cr .( Should read: "forth ")
  cr .( This name:   ") $0E .name .( ")
  cr .( Should read: "forth-wordlist ") cr
  end-transient  code zx exx, exx, jpnext, end-code
  forget-transient

( window-test )

need window need wtype need wcls need ~~ need s+

:noname ( ca len -- )
 0 ~~y @ 3 + at-xy ." x " wx c@ . ." y " wy c@ .
                   ." free " wfreecolumns . ;
' ~~app-info defer!

window my-window

: txt ( -- ca len )
  s" En un lugar de La Mancha de cuyo nombre no quiero"
  s"  acordarme no ha mucho tiempo que..." s+ ;

default-colors cls 7 border

20  5  8 15 set-window 6 paper 0 ink wcls txt wtype
 2  2 22  6 set-window 5 paper 0 ink wcls txt wtype
 0 10 10 12 set-window 2 paper 7 ink wcls txt wtype

home default-colors

( /sconstants-test )

need /sconstants

0                \ end of strings
  here ," kvar"  \ string 4
  here ," tri"   \ string 3
  here ," du"    \ string 2
  here ," unu"   \ string 1
  here ," nul"   \ string 0
/sconstants digitname  constant digitnames

cr .( There are ) digitnames . .( digit names:)
0 digitname cr type
1 digitname cr type
2 digitname cr type
3 digitname cr type cr

( sconstants-test )

need sconstants

0                \ end of strings
  here ," kvar"  \ string 4
  here ," tri"   \ string 3
  here ," du"    \ string 2
  here ," unu"   \ string 1
  here ," nul"   \ string 0
sconstants digitname

0 digitname cr type
1 digitname cr type
2 digitname cr type
3 digitname cr type cr

( associative-list-test )

need associative-list need items
need entry: need centry: need 2entry: need sentry:

associative-list stuff

1887          stuff entry:  year
'E'           stuff centry: letter
s" Saluton"   stuff sentry: hello
314159.       stuff 2entry: pi

cr .( Keys:) cr stuff items cr

cr .( Values: ) cr

s" year"    stuff item . cr
s" letter"  stuff item emit cr
s" hello"   stuff item type cr
s" pi"      stuff item d. cr

( transient-asm-test )

only forth definitions
need order need wordlist-words need .wordname

: info ( -- )
  cr ." here            =" here u.
  cr ." np              =" np@ u.
  cr ." latest-wordlist =" latest-wordlist @ u.
  cr ." limit           =" limit @ u.
  cr ." farlimit        =" farlimit @ u.
  cr ." latest          =" latest dup u. .name
  cr ." current-latest  =" current-latest dup u. .name
  order
  cr ." Continue? (y/n)"
  begin  key dup 'n' = if  abort  then  'y' = until ;  -->

( transient-asm-test )

need transient

cr .( Before transient :)         info  2000 2000 transient
cr .( After transient :)          info need assembler
cr .( Before end-transient :)     info  end-transient
cr .( After end-transient :)      info need n>r
cr .( Before forget-transient :)  info  forget-transient
cr .( After forget-transient :)   info

( transient-test )

: info ( -- )
  cr ." here            =" here u.
  cr ." np              =" np@ u.
  cr ." latest-wordlist =" latest-wordlist @ u.
  cr ." limit           =" limit @ u.
  cr ." farlimit        =" farlimit @ u.
  cr ." Press any key to continue" key drop ;

need transient  cr .( Before transient :) info  100 transient

cr .( After transient :) info

: transient-code ( -- ) ." bla bla bla bla" ;

cr .( Before end-transient :) info end-transient
cr .( After end-transient :) info

: using ( -- ) transient-code ;  cr using  \ works fine

forget-transient  cr .( After forget-transient :) info

cr using  \ works fine

( nest-need-test nnt1 nnt2 nnt3 nnt4 nnt5 nnt6 nnt7 nnt8 nnt9 )

[unneeded] nest-need-test ?\ need nnt1  defer nest-need-test

[unneeded] nnt1 ?\ need nnt2  defer nnt1  exit
[unneeded] nnt2 ?\ need nnt3  defer nnt2  exit
[unneeded] nnt3 ?\ need nnt4  defer nnt3  exit
[unneeded] nnt4 ?\ need nnt5  defer nnt4  exit
[unneeded] nnt5 ?\ need nnt6  defer nnt5  exit
[unneeded] nnt6 ?\ need nnt7  defer nnt6  exit
[unneeded] nnt7 ?\ need nnt8  defer nnt7  exit
[unneeded] nnt8 ?\ need nnt9  defer nnt8  exit
[unneeded] nnt9 ?\ defer nnt9

( doer-test )

need doer

doer test
  cr .( Test 1: ) test

make test  cr ." test 2" ;
  cr .( Test 2: ) test

: change    make test ." test 3" ;and  test ;

change
  cr .( Test 3: ) test

undo test
  cr .( Test 4: ) test

( [:-test-1 [:-test-2 [:-test-3 )

  \ 2016-11-25
  \ 2017-02-01: Update.

need [: need recurse

: [:-test-1 ( -- )
  cr ." latestxt=" [ latestxt ] literal u.
  [: cr ." Quotation latestxt=" [ latestxt ] literal u. ;]
  execute  cr ." The end" ;

: [:-test-2 ( -- )
  cr ." Main"
  [: cr ." Enough? (Y/N)" key lower 'y' = ?exit recurse ;]
  cr ." About to execute..." execute  cr ." The end" ;

: [:-test-3 ( n -- )
  cr ." Main:"
  [: cr ." Count=" dup . 1- ?dup 0= ?exit recurse ;] execute
  cr ." End of quotation" ;

( :switch-test )

  \ 2015-11-15: Test passed
  \ 2016-11-25: Test passed

need :switch need <switch need :noname need >body

: one   ( -- ) ." unu " ;
: two   ( -- ) ." du " ;
: three ( -- ) ." tri " ;
: many  ( n -- ) . ." is too much! " ;

' many :switch numbers
  \ `many` is the default action of the new switch `numbers`

  ' one   1 <switch
  ' two   2 <switch
  ' three 3 <switch  drop

cr 1 numbers 2 numbers 3 numbers 4 numbers

' numbers >body  :noname  ." kvar " ; 4 <switch drop
  \ add a new clause for number 4

cr 1 numbers 2 numbers 3 numbers 4 numbers

( [switch-test )

  \ 2015-11-15: Test passed
  \ 2016-11-25: Test passed

need :switch-test need [switch need [+switch
need runs need run:

[switch sugar-numbers many
  1 runs one  2 runs two  3 runs three  switch]

cr 1 sugar-numbers 3 sugar-numbers 4 sugar-numbers

: four  ." kvar " ;

[+switch sugar-numbers  4 runs four  switch]
  \ add a new clause for number 4

cr 1 sugar-numbers 3 sugar-numbers 4 sugar-numbers

[+switch sugar-numbers  5 run: ." kvin" ;  switch]
  \ add a new unnamed clause for number 5

cr 1 sugar-numbers 4 sugar-numbers 5 sugar-numbers

( alias-test synonym-test )

  \ 2015-11-24

need alias need synonym

' literal alias literal-a
' border alias border-a
' if alias if-a

synonym border-s border
synonym literal-s literal
synonym if-s if

  \ XXX TMP -- alternative `synonym` that uses `alias`
synonym2 border-s2 border
synonym2 literal-s2 literal
synonym2 if-s2 if

  \ : ifa if-a ." yes" then ;   \ "then" error #-4
  \ : ifs if-s ." yes" then ;   \ ok
  \ : ifs2 if-s2 ." yes" then ;   \ "then" error #-4
  \
  \ 1 literal     \ error -14 \ ok
  \ 1 literal-a   \ no error
  \ 1 literal-s   \ error -14 \ ok
  \ 1 literal-s2  \ no error
  \
  \ : zx [ 1 ] literal ;      \ ok
  \ : zx [ 1 ] literal-a ;    \ error #-264
  \ : zx [ 1 ] literal-s ;    \ no error \ ok
  \ : zx [ 1 ] literal-s2 ;   \ error #-264

( {if-test {do-test )

  \ 2015-11-11

: test2 ( n -- )
  {do   dup 5 <   do> ." <5" cr 1+
  |do|  dup 10 <  do> ." <10" cr 1+
  do} drop ;

: test1 ( n1 n2 -- )
  {if   2dup > if> ." >" cr
  |if|  2dup < if> ." <" cr
  if} ;

( options[-test )

: o1 ." option 1" ;  : o2 ." option 2" ;  : o3 ." option 3" ;

: test ( c -- )
  options[
    'a' option o1  'b' option o2  'c' option o3
  ]options  ." end of test" cr ;

: retest ( -- ) 'a' test ." end of retest" cr ;

: o0 ." default" ;

: testd ( c -- )
  options[
    'a' option o1  'b' option o2  'c' option o3
    default-option o0
  ]options ;

( /-test )

  \ 2015-09-22: This test shows that Abersoft Forth's `m/` does
  \ a symmetric division, and so it's equivalent to Forth-94's
  \ `sm/rem`.

  \ From the Forth-94 documentation:

     \ Table 3.4 - Symmetric Division Example

     \ Dividend        Divisor Remainder       Quotient
     \ --------        ------- ---------       --------
     \ 10                 7       3                1
     \ -10                7      -3               -1
     \ 10                -7       3               -1
     \ -10               -7      -3                1

[defined] (/) ?\ defer (/)

: ((/-test)) ( dividend divisor -- )
  >r s>d r> (/) swap . . space ;

: (/-test) ( -- )
  cr  10  7 ((/-test)) -10  7 ((/-test))
      10 -7 ((/-test)) -10 -7 ((/-test)) ;

: /-test ( -- )
  dup ['] m/     ['] (/) defer! (/-test)
      ['] sm/rem ['] (/) defer! (/-test) ;

( exception-test )

  \ Credit:
  \
  \ Code from MPE Forth for TiniARM User Manual.

need catch

: could-fail ( -- c )
  key dup 'q' =
  if  -1 throw  then ;

: do-it ( a b -- c )
  2drop could-fail ;

: try-it ( -- )
  1 2 ['] do-it catch
  if    ( x1 x2 ) 2drop ." There was an exception" cr
  else  ." The character was " emit cr then ;

: retry-it ( -- )
  begin   1 2 ['] do-it catch
  while   ( x1 x2 ) 2drop ." Exception, keep trying" cr
  repeat ( c )
  ." The character was " emit cr ;

( err>ord )

  \ XXX TMP -- `err>ord` tests `error>ordinal`

: err>ord ( -- )
  91 1 ?do  i . i error>ordinal ."  -> " . cr  loop
  286 256 ?do  i . i error>ordinal ."  -> " . cr  loop
  1025 1000 ?do  i . i error>ordinal ."  -> " . cr  loop ;

( type-fields-test )

need type-left need type-center need type-right

s" La Mancha" 2constant text

: ruler ( -- ) home '-' 32 emits home ;

: ready ( -- )
  0 1 at-xy ." Press any key to continue"
  key drop ruler ;

-->

( type-fields-test )

: test ( -- ) page

  ready text text nip type-left
  ready text 32 type-left
  ready text 5 type-left

  ready text text nip type-center
  ready text 32 type-center
  ready text 5 type-center

  ready text text nip type-right
  ready text 32 type-right
  ready text 5 type-right ;

: tc ( ca len1 len2 -- )
  2dup <=> cells type-center-cases + @ >name .name ;

: tr ( ca len1 len2 -- )
  2dup <=> cells type-right-cases + @ >name .name ;

( ?ccase-test ccase0-test )

need ?ccase need ccase0

: .a    ( -- ) ." Letter A" ;
: .b    ( -- ) ." Letter B" ;
: .c    ( -- ) ." Letter C" ;
: .nope ( -- ) ." Nope!" ;
: .end  ( -- ) ."  The End" cr ;

: ?letter ( c -- )
  cr ." ?letter... " key drop
  s" abc" ?ccase  .a .b .c  end?ccase  .end ;

'a' ?letter  'b' ?letter  'c' ?letter  'x' ?letter

: letter0 ( c -- )
  cr ." letter0... " key drop
  s" abc" ccase0 .nope  .a .b .c  endccase0  .end ;

'a' letter0  'b' letter0  'c' letter0  'x' letter0

( ccase-test )

need ccase

: .a    ( -- ) ." Letter A" ;
: .b    ( -- ) ." Letter B" ;
: .c    ( -- ) ." Letter C" ;
: .nope ( -- ) ." Nope!" ;
: .end  ( -- ) ."  The End" cr ;

: letter ( c -- )
  cr ." letter... " key drop
  s" abc" ccase  .a .b .c  .nope  endccase  .end ;

'a' letter  'b' letter  'c' letter  'x' letter

( begincase-test )

need begincase

: test
  begincase
    cr ." press a key ('2' '4' '9' exits) : " key
    '2' of  ." ... 2 "  endof
    '4' of  ." ... 4 "  endof
    '9' of  ." ... 9 "  endof
      dup emit ."  try again"
  repeatcase ;

( jk-test )

need j need k

: jk-test ( -- )
  3 0 ?do
    13 10 ?do
      23 20 ?do
        k . j . i . cr
      loop
    loop
  loop ;

cr jk-test

( color-test )

  \ 2016-05-01

need color need permcolor

: c? ( ca -- ) c@ . ;

: .color ( -- )
  cr ." os-attr-t " os-attr-t c?
  cr ." os-mask-t " os-mask-t c?
  cr ." os-attr-p " os-attr-p c?
  cr ." os-mask-p " os-mask-p c? ;

( search-test )

  \ 2016-05-05

  \ Test the bug recently discovered in the code of `search`
  \ that was adapted from DZX-Forth.

256 constant /long-string
create long-string /long-string allot

: -long-string ( -- ) long-string /long-string blank ;
  \ Blank the long string.

s"  zx " 2constant substring

: place-substring ( n -- )
  -long-string substring rot long-string + 1- swap cmove ;
  \ Place the substring at offset _n_ of the long string.

: search-at ( n -- )
  dup place-substring long-string swap substring search
  .s drop 2drop ;
  \ Search the first _n_ characters of the long string for the
  \ substring, which is placed at offset _n_.

: run ( -- )
  cr ." Search at 128:" 128 search-at
  cr ." Search at 64:" 64 search-at
  cr ." Search at 32:" 32 search-at ;

  \ XXX NOTE: In fact, the substring is found at any position.

( foo3 foo4 )

  \ 2016-05-07

cr .( foo3)
cr .( foo4)

( need-test foo1 foo2 )

  \ 2016-05-07

need foo3 need [if]

[needed] foo1 [if] need foo4
cr .( foo1) exit [then]

cr .( foo2)

( ?(-test )

  \ 2016-05-07

0 dup ?( create zx1a  create zx1b ?)
1 ?(

\ nope
nope

?) ?( : zx1c
 ." zx1c"
 ; : zx1d
 ; ?)

( a>e-test )

  \ 2016-10-26
  \
  \ This test compares the results of the new version of `a>e`,
  \ written in Z80 in the kernel, with the previous version
  \ written in Forth.
  \
  \ 2016-11-13: Update after the renaming done in the kernel
  \ (`a>e` to `far`; `extra-memory` to `far-banks`).

need u>ud

: high-far ( a1 -- a2 )
  u>ud $4000 um/mod  far-banks + c@ bank  $C000 + ;
  \ High-level version of `far`.

: far-test ( -- )
  hex $FFFF 1+ $0000 ?do
    home i 4 .r  i far i high-far <>
    if  cr ." Mismatch in address " u. abort  then
  loop  decimal ;

( lineload-test )

  \ To test `lineload`:
  \ Do `n locate lineload-test lineload` where "n" is the line
  \ number of this block.

  .( line 1 ) cr
  .( line 2 ) cr
  .( line 3 ) cr
  .( line 4 ) cr
  .( line 5 ) cr
  .( line 6 ) cr
  .( line 7 ) cr
  .( line 8 ) cr
  .( line 9 ) cr
  .( line 10 ) cr
  .( line 11 ) cr
  .( line 12 ) cr
  .( line 13 ) cr
  .( line 14 ) cr
  .( line 15 ) cr

( load-section-test )

  \ To test `load-section`:
  \ Do `load-section load-section-test`.

  .( section line 1 ) cr need [if]
  .( section line 2 ) cr
  .( section line 3 ) cr
  .( section line 4 ) cr
  .( section line 5 ) cr
  .( section line 6 ) cr
  .( section line 7 ) cr
  .( section line 8 ) cr
  .( section line 9 ) cr
  .( section line 10 ) cr
  .( section line 11 ) cr
  .( section line 12 ) cr
  .( section line 13 ) cr
  0 [if]  .( section line 14 NOT! ) cr
  .( section line 15 NOT! ) cr

.( section line 16 NOT! ) cr

  \ To test `load-section`:
  \ Do `load-section load-section-test`.

  .( section line 17 NOT! ) cr [else]
  .( section line 18 ) cr
  .( section line 19 ) cr
  .( section line 20 ) cr [then]
  .( section line 21 ) cr
  .( section line 22 ) cr
  .( section line 23 ) cr
  .( section line 24 ) cr
  .( section line 25 ) cr
  .( section line 26 ) cr
end-section
  .( End of section) cr
.( block x line 0)
  .( block x line 1 ) cr
  .( block x line 2 ) cr
  .( block x line 3 ) cr
  .( block x line 4 ) cr
  .( block x line 5 ) cr
  .( block x line 6 ) cr
  .( block x line 7 ) cr
  .( block x line 8 ) cr
  .( block x line 9 ) cr
  .( block x line 10 ) cr
  .( block x line 11 ) cr
  .( block x line 12 ) cr
  .( block x line 13 ) cr
  .( block x line 14 ) cr
  .( block x line 15 ) cr
.( block x+1 line 0)
  .( block x+1 line 1 ) cr
  .( block x+1 line 2 ) cr
  .( block x+1 line 3 ) cr
  .( block x+1 line 4 ) cr
  .( block x+1 line 5 ) cr
  .( block x+1 line 6 ) cr
  .( block x+1 line 7 ) cr
  .( block x+1 line 8 ) cr
  .( block x+1 line 9 ) cr
  .( block x+1 line 10 ) cr
  .( block x+1 line 11 ) cr
  .( block x+1 line 12 ) cr
  .( block x+1 line 13 ) cr
  .( block x+1 line 14 ) cr
  .( block x+1 line 15 ) cr

( XXX TMP -- block for temporary tries)

need thru
blk @ 1+ blk @ 2+ thru

( XXX TMP -- block for temporary tries)

: hello ( -- )

 ;

  \ vim: filetype=soloforth
