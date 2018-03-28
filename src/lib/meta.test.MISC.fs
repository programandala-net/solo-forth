  \ meta.test.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201803282307
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Development tests.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( read-byte-test )

need read-byte need emit-ascii

: read-byte-test ( len fid -- ior )
  swap 0 ?do
    dup read-byte ?dup if nip unloop exit then emit-ascii
    key bl <> if unloop #-28 exit then
  loop drop 0 ;

( file-test )

need create-file need r/w need r/o need w/o need write-file
need close-file need open-file need read-file need rename-file
need delete-file need cat

variable fid  : buf s" XXXXXXXXXX" ;  : file$ s" ZX.ZX" ;

-->

( file-test )

: file-test ( -- )

      \ <------------------------------>
  cr ." Insert a formatted disk into the"
     ." first drive, then press any key." key drop cr ." Go!"

  file$ w/o create-file throw fid ! ." create-file" cr
  s" hola" fid @ write-file throw ." write-file, part 1" cr
  s" y adios" fid @ write-file throw ." write-file, part 2" cr
  fid @ close-file throw ." close-file" cr

  file$ r/o open-file throw fid ! ." open-file" cr
  buf fid @ read-file throw . ." bytes read:" cr
  buf type cr
  fid @ close-file throw ." close-file" cr
  file$ delete-file throw ;

( testing-test )

: blk-line ( -- ca len )
  blk @ block >in @ dup c/l mod - + c/l ;
  \ Return the current line _ca len_ of the block being
  \ interpreted.

: testing ( "ccc" -- )
  ?loading blk-line >in/l /string type cr ->in/l >in +! ;

blk-line cr '<' emit type '>' emit cr ( testing hello )
testing message 1 testing this is not a message
testing message 2
1 2 + .

( write-file-test )

  \ XXX UNDER DEVELOPMENT

need create-file need r/w need open-file need write-file
need read-file need delete-file

: text$ ( -- ca len ) s" En un lugar de La Mancha" ;

: file$ ( -- ca len ) s" test.txt" ;

file$ r/w create-file throw constant file-id

text$ file-id write-file throw

file-id close-file throw

file-id open-file throw !> file-id

( ,udg-block-test )

  \ Credit:
  \
  \ Sample graphic from Nuclear Waste Invaders
  \ (http://programandala.net/en.program.nuclear_waste_invaders.html)
  \ as of 2018-01-08.


need ,udg-block need /udg+

3 cconstant tank-length  1 cconstant tank-height

here tank-length tank-height ,udg-block

  ..........X..X..........
  ...XXXXXX.X..X.XXXXXXX..
  ..XXXXXXXXXXXXXXXXXXXXX.
  .XXXXXXXXXXXXXXXXXXXXXXX
  .XX.X.X.X.X.X.X.X.X.X.XX
  ..XX..XX..XX..XX..XX.XX.
  ...X.XXX.XXX.XXX.XXX.X..
  ....X.X.X.X.X.X.X.X.X... constant tank

: .tank ( -- )
  tank dup emit-udga /udg+ dup emit-udga /udg+ emit-udga ;

cr .( Tank: ) .tank cr

( udg-block-test )

  \ Credit:
  \
  \ Sample graphic from Nuclear Waste Invaders
  \ (http://programandala.net/en.program.nuclear_waste_invaders.html)
  \ as of 2018-01-08.

need udg-block

3 cconstant tank-length  1 cconstant tank-height

0 cconstant tank

tank-length tank-height tank udg-block

..........X..X..........
...XXXXXX.X..X.XXXXXXX..
..XXXXXXXXXXXXXXXXXXXXX.
.XXXXXXXXXXXXXXXXXXXXXXX
.XX.X.X.X.X.X.X.X.X.X.XX
..XX..XX..XX..XX..XX.XX.
...X.XXX.XXX.XXX.XXX.X..
....X.X.X.X.X.X.X.X.X...

: .tank ( -- ) tank dup emit-udg 1+ dup emit-udg 1+ emit-udg ;

cr .( Tank: ) .tank cr

( {if-test )

  \ Credit:
  \
  \ Based on:
  \
  \ M. Edward Borasky, 1996-08-03, "Towards a Discipline of ANS
  \ Forth Programming", published on Forth Dimensions (volume
  \ 18, number 4, pp 5-14), 1996-12.

need {if

: test1 ( x1 x2 --  )
  cr ." {if --- test1: "
  {if  2dup = if> cr over . ." = " dup .
  |if| 2dup > if> cr over . ." > " dup .
  |if| 2dup < if> cr over . ." < " dup .
  if}  2drop ;

5 0 test1 0 5 test1 5 5 test1

: test2 ( x1 x2 --  )
  cr ." {if --- test2: "
  {if  2dup > if> cr over . ." > " dup .
  |if| 2dup < if> cr over . ." < " dup .
  if}  2drop ;

5 0 test2 0 5 test2
5 5 test2 \ must throw exception #-22

( {do-test )

  \ Credit:
  \
  \ Based on:
  \
  \ M. Edward Borasky, 1996-08-03, "Towards a Discipline of ANS
  \ Forth Programming", published on Forth Dimensions (volume
  \ 18, number 4, pp 5-14), 1996-12.

need {do need {if-test

variable x 5 6553 * x !
variable y 5 6551 * y !

: useful ( -- )
  {do  x @ y @ > do> y @ negate x +!
  |do| y @ x @ > do> x @ negate y +!
  do} ;

: test ( -- )
  cr ." Before: x, y = " x ? y ?
  cr ." useful"
  cr ." After:  x, y = " x ? y ? cr ;

( ;code-test )

need assembler need ;code

: borderer ( n -- )
  create c, ;code ( -- ) ( dfa )
    h pop, m a ld, FE out, jpnext, end-code

0 borderer black-border  1 borderer blue-border

blue-border key drop black-border

( 64cpl-fonts-test )

  \ Credit:
  \
  \ The 64-cpl fonts are part of:
  \
  \ 64#4 - 4x8 FONT DRIVER FOR 64 COLUMNS (c) 2007, 2011
  \
  \ Original by Andrew Owen (657 bytes)
  \ Optimized by Crisis (602 bytes)
  \ Reimplemented by Einar Saukas (494 bytes)
  \
  \ https://sites.google.com/site/zxgraph/home/einar-saukas/fonts
  \ http://www.worldofspectrum.org/infoseekid.cgi?id=0027130

need mode-64o need mini-64cpl-font need nbot-64cpl-font
need omn1-64cpl-font need omn2-64cpl-font need owen-64cpl-font

: .ascii ( -- ) 127 32 ?do i emit loop ;

: .font-test ( -- ) .ascii cr
  ." A QUICK BROWN FOX JUMPS OVER THE LAZY DOG!" cr
  ." A quick brown fox jumps over the lazy dog." cr ;

: 64cpl-font-test ( ca len a -- )
  mode-64-font ! mode-64o cr type ." : " .font-test key drop ;

: 64cpl-fonts-test ( -- )
  s" Minix" mini-64cpl-font 64cpl-font-test
  s" n-Bot" nbot-64cpl-font 64cpl-font-test
  s" Omni1" omn1-64cpl-font 64cpl-font-test
  s" Omni2" omn2-64cpl-font 64cpl-font-test
  s" Owen"  owen-64cpl-font 64cpl-font-test ;

( csprite-test )

need /udg* need csprite
create ship-sprite 3 2 * /udg* allot  3 2 ship-sprite csprite

..XX.X.X........X.X.XX..
..XXX.X.X......X.X.XXX..
..XX.....X....X.....XX..
...XX.....XXXX.....XX...
....XX.....XX.....XX....
.....XXX........XXX.....
......XX........XX......
.......XX......XX.......
.......XX......XX.......
........XX....XX........
........XX....XX........ X.........XXXX.........X
X........XXXXXX........X .XXXXXXXXXXXXXXXXXXXXXX.
..........XXXX.......... ...........XX........... -->

( csprite-test )

need binary need /udg*

variable sprite-width

: dump-csprite ( width height a -- )
  rot dup sprite-width ! -rot
  base @ >r binary
  -rot /udg* * 0 ?do
    i sprite-width @ mod 0= if cr then
    dup i + c@ s>d <# # # # # # # # # #> type
  loop
  r> base ! cr ;

: csprite-test ( -- ) 3 2 ship-sprite dump-csprite ;

( f64-test )

  \ Credit:
  \
  \ The 64-cpl fonts are part of:
  \
  \ 64#4 - 4x8 FONT DRIVER FOR 64 COLUMNS (c) 2007, 2011
  \
  \ Original by Andrew Owen (657 bytes)
  \ Optimized by Crisis (602 bytes)
  \ Reimplemented by Einar Saukas (494 bytes)
  \
  \ https://sites.google.com/site/zxgraph/home/einar-saukas/fonts
  \ http://www.worldofspectrum.org/infoseekid.cgi?id=0027130

need mode-64o need file> need owen-64cpl-font

create f64 336 allot

: .ascii ( -- ) 128 bl ?do i emit loop ;

: .text ( -- )
  ." A QUICK BROWN FOX JUMPS OVER THE LAZY DOG." cr
  ." A quick brown fox jumps over the lazy dog." ;

defer show-font ( -- )

: try-f64 ( ca1 len1 ca2 len2 -- )
  mode-64o type space type space cr show-font cr mode-32 ;

: try-f64-file ( ca len -- ) s" File:" 2over f64 0 file> throw
                             f64 mode-64o-font ! try-f64 ;

-->

( f64-test )

: (f64-test ( -- )
  page s" mini.f64" try-f64-file cr
       s" nbot.f64" try-f64-file cr
       s" omn1.f64" try-f64-file cr
       s" omn2.f64" try-f64-file cr
       s" owen.f64" try-f64-file cr
       owen-64cpl-font mode-64o-font !
       s" owen-64cpl-font" s" Word:" try-f64 ;

: continued ( -- )  ." ..." key drop ;

: f64-test1 ( -- ) ['] .ascii ['] show-font defer! (f64-test ;

: f64-test2 ( -- ) ['] .text ['] show-font defer! (f64-test ;

: f64-test ( -- ) f64-test1 continued f64-test2 ;

( wtype-test )

need window need attr@ need attr!
need wltype need wtype need wblank

8 1 21 22 window dup constant test-window current-window !

: wipe ( -- ) attr@ >r 56 attr! wblank r> attr! ;

: quit? ( -- f ) key lower 'y' = ;

: wtype-test ( -- )
  wipe begin s" Hi, it's wtype. Should I quit? " wtype quit?
       until ;

: wltype-test ( -- )
  wipe begin s" Hi, it's wltype. Should I quit? " wltype quit?
       until ;

( sqrt-test )

need baden-sqrt need newton-sqrt need printer

: run ( -- ) cr ." Printing different results" cr
                ." Press BREAK to stop" cr cr printer cr
                ." Number baden-sqrt  newton-sqrt" cr
                ." ------ ----------- -----------" cr
  32768 0 ?do
    i newton-sqrt i baden-sqrt 2dup <>
    if  i 6 .r space 11 .r space 11 .r cr else 2drop then
    break-key? if terminal ." Break." cr leave then
  loop terminal ." End."  ;

( menu-test )

need menu need :noname

:noname ( -- ) unnest unnest ;
:noname ( -- ) 2 border ;
:noname ( -- ) 1 border ;
:noname ( -- ) 0 border ;

create actions> , , , ,

here s" EXIT"  s,
here s" Red"   s,
here s" Blue"  s,
here s" Black" s,

create texts> , , , ,

: menu-pars ( -- a1 a2 ca len col row n1 n2 )
  actions> texts> s" Border" 7 7 14 4 ;

: h ( -- ) home default-colors ;

menu-pars new-menu

( orthodraw-test ortholine-test )

need orthodraw need ortholine need attr! need ms need random

variable delay  300 delay !  variable length  30 length !

2variable center  100 100 center 2!

: ray ( gx gy n1 n2 xt )
  >r center 2@ 2swap length @ r> execute delay @ ms ;

: color ( -- b ) 7 random 1+ ;

: d-ray ( gx gy n1 n2 ) color attr! ['] orthodraw ray ;

: d-test ( -- ) 0  1 d-ray  1  1 d-ray  1 0 d-ray  1 -1 d-ray
                0 -1 d-ray -1 -1 d-ray -1 0 d-ray -1  1 d-ray ;

: l-ray ( gx gy n1 n2 ) ['] ortholine ray ;

: l-test ( -- ) 0  1 l-ray  1  1 l-ray  1 0 l-ray  1 -1 l-ray
                0 -1 l-ray -1 -1 l-ray -1 0 l-ray -1  1 l-ray ;

: run ( -- ) 100 100 center 2! 30 length ! d-test
             130 120 center 2! 50 length ! l-test ;

( gigatype-test )

need gigatype

: run ( -- )
  cls
  8 0 ?do
    17 0 i 3 * tuck at-xy s" GIGATYPE" i gigatype
                    at-xy ." style "   i .
  loop
  key drop home ;

run

( l:-test )

need assembler also assembler need l: previous

code a-call ( a -- )
  #3 rl# jr, nop, #3 rl# c? ?jr,
  #2 call, al#
  nop,
  #2 l: ret,
  #3 l: nop,
  #3 jp, al#
  #2 rl# jr,
  ret,
  end-code

  cr .( Disassemble at ) ' a-call u. cr .( to see the result)

( udg-group-test )

need udg-group need set-udg

here 5 8 * allot set-udg

5 1 0 udg-group

..XXXX.. ..XXXX.. ..XXXX.. ..XXXX.. ..XXXX..
.XXXXXX. .XXXXXX. .XXXXXX. .XXXXXX. .X.XXXX.
XXXXXXXX XXXXXXXX XXXXXXXX X.XXXXXX X.XXXXXX
XXXXXXXX XXXXXXXX X.XXXXXX X.XXXXXX XXXXXXXX
XXXXXXXX X.XXXXXX X.XXXXXX XXXXXXXX XXXXXXXX
XX..XXXX XX.XXXXX XXXXXXXX XXXXXXXX XXXXXXXX
.XXXXXX. .XXXXXX. .XXXXXX. .XXXXXX. .XXXXXX.
..XXXX.. ..XXXX.. ..XXXX.. ..XXXX.. ..XXXX..

: run ( -- ) cr 5 0 ?do i emit-udg loop cr ;

run

( udg-block-test )

need udg-block need set-udg

create udg-font 5 8 * allot udg-font set-udg

5 1 0 udg-block

....XXXXXXXXXXXXXXXXXXXXXXXXXXX.........
...XXXXXXXXXXXXXXXXXXXXXXXXXXX..........
..XXXXXXXXXXXXXXXXXXXXXXXXXXX...........
.XXXXXXXXXXXXXXXXXXXXXXXXXXX............
XXXXXXXXXXXXXXXXXXXXXXXXXXX.............
XXXXXXXXXXXXXXXXXXXXXXXXXX..............
XXXXXXXXXXXXXXXXXXXXXXXXX...............
....XXXXXXXXXXXXXXXXXXXXXXXXXXX.........

: run ( -- ) cr 5 0 ?do i emit-udg loop cr ;

run

( local-test )

need local need clocal need 2local need cvariable need rnd
need c? need 2?

variable tmp cvariable ctmp 2variable 2tmp

: .tmp ( -- ) cr ." tmp  = "  tmp  ? cr
                 ." ctmp = " ctmp c? cr
                 ." 2tmp = " 2tmp 2? cr ;

: set-tmp ( -- ) rnd tmp ! rnd ctmp c! rnd rnd 2tmp 2! ;

: (local-test ( -- )
  tmp local ctmp clocal 2tmp 2local
  1001 tmp ! 101 ctmp c! 100001. 2tmp 2!
  cr ." Local values:" .tmp ;

: local-test ( -- ) set-tmp
                    cr ." Current values:"  .tmp (local-test
                    cr ." Restored values:" .tmp ;

  \ local-test

( anon-test )

need anon  here anon> ! 5 cells allot

: run5  ( x[n-1]..x[0] n -- )
  set-anon cr
  [ 0 ] anon ?                   \ display first parameter
  123 [ 0 ] anon ! [ 0 ] anon ?  \ display 123
  [ 1 ] anon ?                   \ display second parameter
  [ 2 ] anon ?                   \ display third parameter
  555 [ 2 ] anon ! [ 2 ] anon ?  \ display 555
  [ 3 ] anon ?                   \ display fourth parameter
  [ 4 ] anon ? ;                 \ display fifth parameter

400 300 200 100 000 5 run5

here anon> ! 2 cells allot

: run2  ( x[n-1]..x[0] n -- )
  set-anon cr [ 0 ] anon ? [ 1 ] anon ? ;

2002 1001 2 run2

( arguments-test )

need arguments need results need toarg need +toarg need 3dup

: val-test ( length width height -- length' volume surface )
  3 arguments   l0 l1 * toarg l5       \ surface
                l5 l2 * toarg l4       \ volume
                $2000 +toarg l0        \ length+$2000
                l4 toarg l1            \ volume
                l5 toarg l2            \ surface
  3 results ;

: var-test ( length width height -- length' volume surface )
  3 arguments   l0 @ l1 @ * l5 !   \ surface
                l5 @ l2 @ * l4 !   \ volume
                $2000 l0 +!        \ length+$2000
                l4 @ l1 !            \ volume
                l5 @ l2 !            \ surface
  3 results ; -->

( arguments-test )

defer test

: .results ( length width height -- )
  ." OUTPUT" cr ."   Surface:" . cr
                ."   Volume: " . cr
                ."   Length + 8192: " . cr ;


: run ( length width height -- )
  3dup 3dup
  cr ." INPUT"  cr ."   Length: " . cr
                   ."   Width:  " . cr
                   ."   Height: " . cr
  ." Value-like arguments:"    cr val-test .results
  arg-default-action off  arg-action off
  \ ['] noop dup arg-default-action ! ['] arg-action defer!
  ." Variable-like arguments:" cr var-test .results ;

( dzx7-test )

need dzx7s need dzx7t need dzx7m need file> need case
create compressed 6912 allot

: run ( -- )

  page ." Press any key to load the" cr
       ." compressed screen from the" cr
      ." first disk drive. 'Q' to quit." key 'q' = ?exit

  1 set-drive throw s" img.zx7"  compressed 0 file> throw

  page  begin home
          ." Decompress with dzx7[S/T/M]" cr ." or [Q]uit" cr
          key lower case
            's' of page compressed 16384 dzx7s endof
            't' of page compressed 16384 dzx7t endof
            'm' of page compressed 16384 dzx7m endof
            'q' of quit endof
          endcase again ; run

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

( >name-test0 )

  \ 2017-01-20
  \
  \ 2017-12-15: This test is obsolete: The new version of
  \ `>name` searches from newest to oldest definitions.

need far, need >name

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
 ?\ .name
 ?\ .( not found!)  \ or simply hang in an endless searching!

( >name-test1 )

  \ 2017-12-15

only forth definitions

need >name need >name/order need >oldest-name/order need alias
need >oldest-name need >oldest-name/fast

: the-original ;

wordlist constant the-wordlist  the-wordlist set-current

' the-original alias the-alias

only forth definitions

' the-original >name cr .( "the-alias": ) .name

' the-original >name/order cr .( "the-original": ) .name

the-wordlist >order ' the-original >oldest-name/order
cr .( "the-original": ) .name previous

' the-original >oldest-name cr .( "the-original": ) .name

' the-original >oldest-name/fast cr .( "the-original": ) .name

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

unneeding nest-need-test ?\ need nnt1  defer nest-need-test

unneeding nnt1 ?\ need nnt2  defer nnt1  exit
unneeding nnt2 ?\ need nnt3  defer nnt2  exit
unneeding nnt3 ?\ need nnt4  defer nnt3  exit
unneeding nnt4 ?\ need nnt5  defer nnt4  exit
unneeding nnt5 ?\ need nnt6  defer nnt5  exit
unneeding nnt6 ?\ need nnt7  defer nnt6  exit
unneeding nnt7 ?\ need nnt8  defer nnt7  exit
unneeding nnt8 ?\ need nnt9  defer nnt8  exit
unneeding nnt9 ?\ defer nnt9

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
  \ 2017-12-10: Test passed

need :switch need :noname need >body

: one   ( -- )   ." unu " ;
: two   ( -- )   ." du "  ;
: three ( -- )   ." tri " ;
  \ clauses

: many  ( n -- ) . ." is too much! " ;
  \ default action

' many :switch .number

  ' one   1 <switch
  ' two   2 <switch
  ' three 3 <switch  drop

cr 1 .number 2 .number 3 .number 4 .number

' .number >body  :noname  ." kvar " ; 4 <switch drop
  \ add a new clause for number 4

cr 1 .number 2 .number 3 .number 4 .number

( [switch-test )

  \ 2015-11-15: Test passed
  \ 2016-11-25: Test passed
  \ 2017-12-10: Test passed

need [switch need [+switch need runs need run:

: one   ( -- )   ." unu " ;
: two   ( -- )   ." du "  ;
: three ( -- )   ." tri " ;
  \ clauses

: many  ( n -- ) . ." is too much! " ;
  \ default action

[switch .sugar-number many
  1 runs one  2 runs two  3 runs three  switch]

cr 1 .sugar-number 3 .sugar-number 4 .sugar-number

: four  ." kvar " ;

[+switch .sugar-number  4 runs four  switch]
  \ add a new clause for number 4

cr 1 .sugar-number 3 .sugar-number 4 .sugar-number

[+switch .sugar-number  5 run: ." kvin" ;  switch]
  \ add a new unnamed clause for number 5

cr 1 .sugar-number 4 .sugar-number 5 .sugar-number

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

defer (/) ( d n1 -- n2 n3 )

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
  repeat  ( c )
  ." The character was " emit cr ;

( err>ord )

  \ XXX TMP -- `err>ord` tests `error>ordinal`

: err>ord ( -- )
  91 1 ?do  i . i error>ordinal ."  -> " . cr  loop
  286 256 ?do  i . i error>ordinal ."  -> " . cr  loop
  1025 1000 ?do  i . i error>ordinal ."  -> " . cr  loop ;

( type-fields-test )

need type-left need type-center need type-right need >name

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

need color need permcolor need c?

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

  \ ===========================================================
  \ Change log

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
  \
  \ 2017-03-18: Add `arguments-test` and `anon-test`.
  \
  \ 2017-03-19: Finish `anon-test`. Add `local-test`. Add
  \ `udg-block-test` and `udg-group-test`.
  \
  \ 2017-03-21: Add `l:-test`. Fix `zx7s-test` after the
  \ version used in the `zx7s` module.
  \
  \ 2017-03-22: Rename `zx7s-test` to `zx7-test` and support
  \ also `zx7t` and `zx7m`.
  \
  \ 2017-03-23: Update the names of the ZX7 decompressors and
  \ test.
  \
  \ 2017-03-28: Improve `dzx7-test`. Add `gigatype-test`.
  \
  \ 2017-03-29: Add `orthodraw-test`, `ortholine-test`,
  \ `menu-test`, `sqrt-test`.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2017-05-08: Update: `display` has been renamed to
  \ `terminal` in the kernel.
  \
  \ 2017-05-13: Add `wtype-test` and `wltype-test`.
  \
  \ 2017-05-15: Add `f64-test`.
  \
  \ 2017-05-21: Add `csprite-test`.
  \
  \ 2017-12-05: Add `64cpl-fonts-test`.
  \
  \ 2017-12-09: Remove useless `[defined] (/) ?\`.
  \
  \ 2017-12-10: Add `;code-test`.
  \
  \ 2017-12-11: Check and modify `:switch-test` and
  \ `[switch-test`. Add `{if-test` and `{do-test`.
  \
  \ 2017-12-12: Need `>name`, which has been moved to the
  \ library. Need `c?`, which has been moved to
  \ <memory.MISC.fs>.
  \
  \ 2017-12-15: Rename `>name-test` `>name-test0` and fix it.
  \ Add `>name-test1`.
  \
  \ 2017-12-17: Add `>oldest-name` and `>oldest-name/fast`, to
  \ `>name-test1`.
  \
  \ 2018-01-08: Add `udg-block-test`, `,udg-block-test`.
  \
  \ 2018-02-14: Start `file-write-test`.
  \
  \ 2018-03-05: Improve `{if-test`.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-08: Improve documentation of `menu-test`.
  \
  \ 2018-03-09: Update stack notation "x y" to "col row".
  \
  \ 2018-03-11: Add `testing-test`. (`testing` is part of
  \ `ttester`).
  \
  \ 2018-03-13: Add `file-test`.
  \
  \ 2018-03-28: Move `read-byte-test` from the +3DOS module.
  \ Add `clocal` and `2local` to `local-test` and improve it.

  \ vim: filetype=soloforth
