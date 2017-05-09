  \ meta.benchmark.ultimate.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201705091046
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ The ultimate Forth Benchmark
  \ (https://theultimatebenchmark.org/).
  \
  \ Unless otherwise stated, benchmark results were obtained on
  \ a ZX Spectrum 128 with G+DOS, emulated by Fuse.

  \ ===========================================================
  \ Authors

  \ Original code by Hans Bezemer, Marcel Hendrix, Carsten
  \ Strotmann et al.

  \ Adapted from <https://theultimatebenchmark.org/> by Marcos
  \ Cruz (programandala.net), 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( doint-bench )

need bench{ need }bench.

32000 constant intMax

variable intResult

: DoInt-bench ( -- )
  bench{
  1 dup intResult dup >r !
  begin
    dup intMax <
  while
    dup negate r@ +! 1+
    dup r@ +! 1+
    r@ @ over * r@ ! 1+
    r@ @ over / r@ ! 1+
  repeat
  r> drop drop }bench. ;

  \ Date       Frames Seconds Note
  \ ---------- ------ ------- -------------------------------
  \ 2017-05-06   1295   25.90
  \ 2017-05-09   1290   25.80 `next` routine after `do_colon`
  \ 2017-05-09   1259   25.18 `next` routine after `exit`
  \ 2017-05-09   1258   25.16 `next` routine after both of them

( fib1-bench )

need bench{ need }bench. need recurse need do

: fib1 ( n1 -- n2 )
  dup 2 < if drop 1 exit then
  dup  1- recurse
  swap 2- recurse  + ;

: fib1-bench ( -- )
  bench{ 1000 0 do i fib1 drop loop }bench. ;

  \ Date        Frames Seconds
  \ ----------  ------ -------

( fib2-bench )

need bench{ need }bench. need recurse need do

: fib2 ( n1 -- n2 )
   0 1 rot 0 do
      over + swap loop
   drop ;

: fib2-bench ( -- )
  bench{ 1000 0 do i fib2 drop loop }bench. ;

  \ Date       Frames Seconds Note
  \ ---------- ------ ------- -------------------------------
  \ 2017-05-06   4412   88.24
  \ 2017-05-09   4386   87.72 `next` routine after `do_colon`
  \ 2017-05-09   4270   85.40 `next` routine after `exit`
  \ 2017-05-09   4271   85.42 `next` routine after both of them

( nesting-bench )

 \ Forth nesting (NEXT) Benchmark                     cas20101204

need bench{ need }bench.

: bottom ; : 1st bottom bottom ; : 2nd 1st 1st ;
: 3rd 2nd 2nd ; : 4th 3rd 3rd ; : 5th 4th 4th ; : 6th 5th 5th ;
: 7th 6th 6th ; : 8th 7th 7th ; : 9th 8th 8th ;
: 10th 9th 9th ; : 11th 10th 10th ; : 12th 11th 11th ;
: 13th 12th 12th ; : 14th 13th 13th ; : 15th 14th 14th ;
: 16th 15th 15th ; : 17th 16th 16th ; : 18th 17th 17th ;
: 19th 18th 18th ; : 20th 19th 19th ; : 21th 20th 20th ;
: 22th 21th 21th ; : 23th 22th 22th ; : 24th 23th 23th ;
: 25th 24th 24th ;

: 32million ( -- ) cr ." 32 million nest/unnest operations:" cr
                   bench{ 25th }bench. ;

: 1million ( -- ) cr ." 1 million nest/unnest operations:" cr
                  bench{ 20th }bench. ;

cr .( Enter 1million or 32million) cr

  \ 1million :

  \ Date        Frames Seconds Comp. Note
  \ ----------  ------ ------- ----- ----
  \ 2017-05-06    8382  167.64  1.00  (1)
  \ 2017-05-09    8070  161.40  0.96  (2)
  \ 2017-05-09    8007  160.14  0.95  (3)
  \ 2017-05-09    7719  154.38  0.93  (4)

  \ 32million :

  \ Date        Frames Seconds Comp. Note
  \ ----------  ------ ------- ----- ----
  \ 2017-05-06  268231 5364.62  1.00  (1)
  \ 2017-05-09  258249 5164.99  0.96  (2)
  \ 2017-05-09  256226 5124.52  0.95  (3)
  \ 2017-05-09  246994 4939.88  0.92  (4)
  \ 2017-05-09  246993 4939.86  0.92  (5)

  \ Notes:
  \
  \ 1: `next` routine apart
  \ 2: `next` routine after `do_colon`
  \ 3: `next` routine after `exit`
  \ 4: `next` routine after both of them
  \ 5: `next` routine after both of them but `push_hlde` preserved

( memmove-bench )

 \ Forth Memory Move Benchmark                       cas 20101204

need bench{ need }bench. need do

8192 constant bufsize

variable buf1 here bufsize 1+ allot buf1 !

variable buf2 here bufsize 1+ allot buf2 !

: test-cmove 49 0 do buf1 @ buf2 @ bufsize cmove loop ;

: test-cmove> 49 0 do buf2 @ buf1 @ bufsize cmove> loop ;

: test-move> 49 0 do buf1 @ buf2 @ bufsize move loop ;

: test-<move 49 0 do buf2 @ buf1 @ bufsize move loop ;

: memmove-bench ( -- )
  bench{ test-cmove test-cmove> test-move> test-<move }bench. ;

  \ Date       Frames Seconds Note
  \ ---------- ------ ------- -------------------------------
  \ 2017-05-06    522   10.44
  \ 2017-05-09    521   10.42 `next` routine after `do_colon`
  \ 2017-05-09    522   10.44 `next` routine after `exit`
  \ 2017-05-09    521   10.42 `next` routine after both of them

( countbits-bench )

 \ Forth Benchmark - count bits in byte              cas 20101204

need bench{ need }bench. need do need 2/

decimal

variable cnt

: (countbits-bench ( uu -- #bits )
  cnt off
  8 0 do dup $01010101 and cnt +!
         2/
  loop drop
  0 cnt 4 bounds do i c@ + loop ;

: countbits-bench ( -- )
  bench{ 8192 0 do i (countbits-bench . loop }bench. ;

  \ XXX TODO -- Confirm the original source. It reads `8192
  \ do`.

  \ Date       Frames Seconds Note
  \ ---------- ------ ------- -------------------------------
  \ 2017-05-06   8882  177.64
  \ 2017-05-09   8849  176.98 `next` routine after `do_colon`
  \ 2017-05-09   8725  174.50 `next` routine after `exit`
  \ 2017-05-09   8688  173.76 `next` routine after both of them

( sieve-bench )

  \ Sieve Benchmark -- the classic Forth benchmark    cas 20101204

need bench{ need }bench. need do

8192 constant size  variable flags 0 flags !  size allot

: sieve-bench ( -- )
  bench{
  flags size 1 fill  ( set array )
  0 ( 0 count ) size 0
  do flags i + c@
    if i dup + 3 + dup i +
       begin  dup size <
       while  0 over flags + c!  over +  repeat
       drop drop 1+
    then
  loop . ." Primes" cr }bench. ;

  \ Date       Frames Seconds Note
  \ ---------- ------ ------- -------------------------------
  \ 2017-05-06    420    8.40
  \ 2017-05-09    419    8.38 `next` routine after `do_colon`
  \ 2017-05-09    409    8.18 `next` routine after `exit`
  \ 2017-05-09    410    8.20 `next` routine after both of them

( gcd1-bench )

  \ gcd - greatest common divisor                     cas 20101204

need bench{ need }bench. need do need j

: (gcd1-bench ( a b -- gcd )
  over if
    begin
      dup while
         2dup u> if swap then over -
    repeat drop else
    dup if nip else 2drop 1 then
  then ;

: gcd1-bench ( -- )
  bench{
    100 0 do 100 0 do j i (gcd1-bench drop loop loop
  }bench. ;

  \ Date       Frames Seconds Note
  \ ---------- ------ ------- -------------------------------
  \ 2017-05-06   2166   43.32
  \ 2017-05-09   2159   43.18 `next` routine after `do_colon`
  \ 2017-05-09   2098   41.96 `next` routine after `exit`
  \ 2017-05-09   2097   41.94 `next` routine after both of them

( gcd2-bench )

  \ another gcd O(2) runtime speed cas 20101204

need bench{ need }bench. need d0= need j

 : (gcd2-bench ( a b -- gcd )
   2dup        d0= if  2drop 1 exit   then
   dup          0= if   drop   exit   then
   swap dup     0= if   drop   exit   then
   begin  2dup -
   while  2dup < if over -
                 else swap over - swap
                 then
   repeat nip ;

: gcd2-bench ( -- )
  bench{
  100 0 do 100 0 do j i (gcd2-bench drop loop loop
  }bench. ;

  \ Date       Frames Seconds Note
  \ ---------- ------ ------- -------------------------------
  \ 2017-05-06   2761   55.22
  \ 2017-05-09   2740   54.80 `next` routine after `do_colon`
  \ 2017-05-09   2666   53.32 `next` routine after `exit`
  \ 2017-05-09   2665   53.30 `next` routine after both of them

( takeuchi-bench )

  \ takeuchi benchmark in volksForth Forth-83
  \ see [7]http://en.wikipedia.org/wiki/Tak_(function)

need bench{ need }bench. need do need pick need recurse

decimal

: 3dup ( x1 x2 x3 -- x1 x2 x3 x1 x2 x3 )
  2 pick 2 pick 2 pick ;

: tak ( x y z -- t )
  over 3 pick < negate if nip nip exit then
  3dup rot 1- -rot recurse >r
  3dup swap 1- -rot swap recurse >r
            1- -rot recurse
  r> swap r> -rot recurse ;

: takeuchi-bench ( -- )
  bench{ 0 1000 0 do drop 18 12 6 tak loop }bench. ;

  \ Date       Frames Seconds Note
  \ ---------- ------ ------- -------------------------------
  \ 2017-05-06     25    0.50
  \ 2017-05-09     25    0.50 `next` routine after `do_colon`
  \ 2017-05-09     25    0.50 `next` routine after `exit`
  \ 2017-05-09     25    0.50 `next` routine after both of them

( 6502emu-bench )

  \ A simple 6502 emulattion benchmark                         cas
  \ only 11 opcodes are implemented. The memory layout is:
  \  2kB RAM at 0000-07FF, mirrored throughout 0800-7FFF
  \ 16kB ROM at 8000-BFFF, mirrored at C000

need bench{ need }bench. need do  decimal

create ram 2048 allot   : >ram $7FF  and ram + ;
create rom 16384 allot  : >rom $3FFF and rom + ;
  \ 6502 registers
variable reg-a   variable reg-x  variable reg-y
variable reg-s   variable reg-pc  : reg-pc+ reg-pc +! ;
  \ 6502 flags
variable flag-c  variable flag-n   variable cycle
variable flag-z  variable flag-v  : cycle+ cycle +! ;
hex
: w@ dup c@ swap 1+ c@ 100 * or ;
: cs@ c@ dup 80 and if 100 - then ;

: read-byte ( address -- )
  dup 8000 < if >ram c@ else >rom c@ then ;
: read-word ( address -- )
  dup 8000 < if >ram w@ else >rom w@ then ; -->

( 6502emu-bench )

: dojmp ( JMP aaaa ) reg-pc @ >rom w@ reg-pc ! 3 cycle+ ;
: dolda ( LDA aa )
  reg-pc @ >rom c@ ram + c@ dup dup reg-a !
  flag-z ! 80 and flag-n ! 1 reg-pc+ 3 cycle+ ;
: dosta ( STA aa )
  reg-a @ reg-pc @ >rom c@ ram + c! 1 reg-pc+ 3 cycle+ ;
: dobeq ( BEQ <aa )
  flag-z @ 0= if reg-pc @ >rom cs@ 1+ reg-pc+
              else 1 reg-pc+ then 3 cycle+ ;
: doldai ( LDA #aa )
  reg-pc @ >rom c@ dup dup reg-a ! flag-z ! 80 and flag-n !
  1 reg-pc+ 2 cycle+ ;
: dodex ( DEX )
  reg-x @ 1- FF and dup dup reg-x ! flag-z ! 80 and flag-n !
  2 cycle+ ; -->

( 6502emu-bench )

: dodey ( DEY )
  reg-y @ 1- ff and dup dup reg-y ! flag-z ! 80 and flag-n !
  2 cycle+ ;
: doinc ( INC aa )
  reg-pc @ >rom c@ ram + dup c@ 1+ FF and dup -rot swap c! dup
  flag-z ! 80 and flag-n !  1 reg-pc+ 3 cycle+ ;
: doldy ( LDY aa )
  reg-pc @ >rom c@ dup dup reg-y ! flag-z ! 80 and flag-n !
  1 reg-pc+ 2 cycle+ ;
: doldx ( LDX #aa )
  reg-pc @ >rom c@ dup dup reg-x ! flag-z ! 80 and flag-n !
  1 reg-pc+ 2 cycle+ ;
: dobne ( BNE <aa )
  flag-z @ if reg-pc @ >rom cs@ 1+ reg-pc+ else 1 reg-pc+ then
  3 cycle+ ; -->

( 6502emu-bench )

: 6502emu ( cycles -- )
  begin cycle @ over  < while
    reg-pc @ >rom c@ 1 reg-pc+
    dup 4C = if dojmp then      dup A5 = if dolda then
    dup 85 = if dosta then      dup F0 = if dobeq then
    dup D0 = if dobne then      dup A9 = if doldai then
    dup CA = if dodex then      dup 88 = if dodey then
    dup E6 = if doinc then      dup A0 = if doldy then
        A2 = if doldx then      repeat drop ;

-->

( 6502emu-bench )

create testcode
  A9 c, 00 c,  \ start: LDA #0
  85 c, 08 c,  \        STA 08
  A2 c, 0A c,  \        LDX #10
  A0 c, 0A c,  \ loop1: LDY #10
  E6 c, 08 c,  \ loop2: INC 08
  88 c,        \        DEY
  D0 c, FB c,  \        BNE loop2
  CA c,        \        DEX
  D0 c, F6 c,  \        BNE loop1
  4C c, 00 c, 80 c, \   JMP start

: init-vm 13 0 do i testcode + c@ i rom + c! loop
          0 cycle ! 8000 reg-pc ! ;                     decimal

: 6502emu-bench ( -- )
  bench{ $100 0 do init-vm 6502 6502emu loop }bench. ;

  \ XXX TODO -- Confirm "&6502" means decimal 6502.

  \ Date       Frames Seconds Comp. Note
  \ ---------- ------ ------- ----- ---------------------------------
  \ 2017-05-06  94384 1887.68  1.00
  \ 2017-05-09  93825 1876.50  0.99 `next` routine after `do_colon`
  \ 2017-05-09  90843 1816.86  0.96 `next` routine after `exit`
  \ 2017-05-09  90348 1806.96  0.95 `next` routine after both of them

  \ ===========================================================
  \ Change log

  \ 2017-05-06: Start.  Adapt the benchmarks from
  \ https://theultimatebenchmark.org/ and run them.
  \
  \ 2017-05-08: Improve module description.
  \
  \ 2017-05-09: Run the benchmarks to test moving/copying the
  \ code of `next` in the kernel and note the results.

  \ vim: filetype=soloforth

