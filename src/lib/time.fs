  \ time.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201711281200
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to time.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( seconds ?seconds ms )

[unneeded] seconds ?\ need ms : seconds ( u -- ) 1000 * ms ;

  \ doc{
  \
  \ seconds ( u -- )
  \
  \ Wait at least _u_ seconds.
  \
  \ See also: `?seconds`, `ms`, `ticks`.
  \
  \ }doc

[unneeded] ?seconds ?( need ?ticks-pause need ticks/second
: ?seconds ( u -- ) ticks/second * ?ticks-pause ; ?)

  \ doc{
  \
  \ ?seconds ( u -- )
  \
  \ Wait at least _u_ seconds or until a key is pressed.
  \
  \ See also: `seconds`, `ms`, `?ticks`.
  \
  \ }doc

[unneeded] ms ?( need assembler

code ms ( u -- )
  d pop, d tstp, nz? rif
    rbegin  #171 a ld#,  rbegin  nop, a dec,  z? runtil
           d decp,  d tstp,
    z? runtil
  rthen  jpnext, end-code ?)

  \ Credit:
  \ Code adapted from v.Forth.

  \ XXX TODO -- support for multitasking (see the
  \ implementation in Z88 CamelForth)

  \ doc{
  \
  \ ms ( u -- )
  \
  \ Wait at least _u_ ms (miliseconds).
  \
  \ Origin: Forth-94 (FACILITY EXT), Forth-202 (FACILITY
  \ EXT).
  \
  \ See also: `seconds`, `ticks-pause`.
  \
  \ }doc

( ticks set-ticks reset-ticks )

[unneeded] ticks ?( need os-frames

: ticks ( -- d )
  os-frames @ [ os-frames cell+ ] literal c@ ; ?)

  \ doc{
  \
  \ ticks ( -- d )
  \
  \ Return the current count of clock ticks _d_, which is
  \ updated by the OS.
  \
  \ See also: `set-ticks`, `reset-ticks`, `os-frames`,
  \ `ticks/second`, `bench{`.
  \
  \ }doc

[unneeded] set-ticks ?( need os-frames

: set-ticks ( d -- )
  [ os-frames cell+ ] literal c! os-frames ! ; ?)

  \ doc{
  \
  \ set-ticks ( d -- )
  \
  \ Set the system clock to _d_ ticks.
  \
  \ See also: `ticks`, `reset-ticks`, `os-frames`,
  \ `ticks/second`, `bench{`.
  \
  \ }doc

[unneeded] reset-ticks

?\ need set-ticks : reset-ticks ( -- ) 0. set-ticks ;

  \ doc{
  \
  \ reset-ticks ( -- )
  \
  \ Reset the system clock to zero ticks.
  \
  \ See: `ticks`, `set-ticks`, `os-frames`, `ticks/second`,
  \ `bench{`.
  \
  \ }doc

( ticks/second ticks>cs ticks>ms ticks>seconds )

[unneeded] ticks/second ?\ 50 cconstant ticks/second
  \ XXX TODO -- Calculate after the lina Forth system.

  \ doc{
  \
  \ ticks/second ( -- n )
  \
  \ Return the number _n_ of clock ticks per second.
  \
  \ See also: `ticks>seconds`, `ticks>cs`, `ticks>ms`, `ticks`.
  \
  \ }doc

[unneeded] ticks>cs ?( need ticks/second need % need m+

: ticks>cs ( d1 -- d2 )
  ticks/second m/ 100 m* rot ticks/second % m+ ; ?)
  \ XXX TODO -- Improve after the lina Forth system.

  \ doc{
  \
  \ ticks>cs ( d1 -- d2 )
  \
  \ Convert clock ticks _d1_ to centiseconds _d2_.
  \
  \ See also: `ticks>seconds`, `ticks>ms`, `ticks/seconds`,
  \ `ticks`.
  \
  \ }doc

[unneeded] ticks>ms ?( need ticks/second

: ticks>ms ( d1 -- d2 )
  ticks/second m/ 1000 m*
  rot ticks/second 1000 swap */ m+ ; ?)
  \ XXX TODO -- Improve after the lina Forth system.

  \ doc{
  \
  \ ticks>ms ( d1 -- d2 )
  \
  \ Convert clock ticks _d1_ to milliseconds _d2_.
  \
  \ See also: `ticks>seconds`, `ticks>cs`, `ticks/seconds`,
  \ `ticks`.
  \
  \ }doc

[unneeded] ticks>seconds ?( need ticks/second need m*/

: ticks>seconds ( d1 -- d2 ) 1 ticks/second m*/ ; ?)
  \ XXX TODO -- Improve after the lina Forth system.

  \ doc{
  \
  \ ticks>seconds ( d -- n )
  \
  \ Convert clock ticks _d_ to seconds _n_.
  \
  \ See also: `ticks>cs`, `ticks>ms`, `ticks/seconds`,
  \ `ticks`.
  \
  \ }doc

( ?ticks-pause ticks-pause pause )

[unneeded] ?ticks-pause ?( need os-frames

: ?ticks-pause ( u -- )
  os-frames @ +
  begin  dup os-frames @ u< key? or  until drop ; ?)

  \ XXX TODO -- multitasking

  \ doc{
  \
  \ ?ticks-pause ( u -- )
  \
  \ Stop execution during at least _u_ clock ticks, or until a
  \ key is pressed.
  \
  \ See: `ticks-pause`, `pause`, `os-frames`, `?seconds`,
  \ `ticks/second`.
  \
  \ }doc

[unneeded] ticks-pause ?( need os-frames

: ticks-pause ( u -- )
  os-frames @ +  begin  dup os-frames @ u<  until drop ; ?)

  \ XXX TODO -- multitasking

  \ doc{
  \
  \ ticks-pause ( u -- )
  \
  \ Stop execution during at least _u_ clock ticks.
  \
  \ See: `?ticks-pause`, `pause`, `os-frames`, `seconds`, `ms`,
  \ `ticks/second`.
  \
  \ }doc

  \ Alternative definition in assembler
  \
  \ XXX TODO -- rewrite with Z80 opcodes

  \ need assembler

  \ code ticks-pause ( u -- )
  \   d pop, b push,
  \   rbegin  halt, d decp, d tstp,  z? runtil
  \   b pop, jpnext, end-code ?)

  \ Credit:
  \
  \ Code adapted from Spectrum Forth-83.

  \ XXX FIXME -- `0 ticks-pause` does `$FFFF ticks-pause`
  \ XXX TODO -- multitasking

[unneeded] pause ?( need ?ticks-pause

: pause ( u -- )
  ?dup if ?ticks-pause exit then  begin key? until ; ?)

  \ doc{
  \
  \ pause ( u -- )
  \
  \ If _u_ 0 zero, stop execution until a key is pressed.
  \ Otherwise stop execution during at least _u_ clock ticks,
  \ or until a key is pressed.
  \
  \ ``pause`` is a convenience that works like the homonymous
  \ keyword of Sinclair BASIC.
  \
  \ See: `ticks-pause`, `?ticks-pause`, `os-frames`,
  \ `?seconds`, `ticks/second`.
  \
  \ }doc

  \ XXX TODO -- Rename `pause` to `basic-pause` or something,
  \ when the multitasking `pause` will be implemented.

( leapy-year? date set-date get-date )

[unneeded] leapy-year? ?(

: leapy-year? ( n -- f )
  dup 400 mod 0= if  drop true   exit  then
  dup 100 mod 0= if  drop false  exit  then
        4 mod 0= if       false  exit  then  false ; ?)

  \ Credit:
  \
  \ Code written by Wil Baden, published on Forth Dimensions
  \ (volume 8, number 5, page 31, 1987-01).

  \ doc{
  \
  \ leapy-year? ( n -- f )
  \
  \ Is _n_ a leapy year?
  \
  \ See also: `set-date`.
  \
  \ }doc

  \ Alternative implementation:

  \ need thiscase

  \ : leapy-year? ( n -- f )
  \   thiscase 400 mod 0= ifcase  true   exitcase
  \   thiscase 100 mod 0= ifcase  false  exitcase
  \   thiscase   4 mod 0= ifcase  true   exitcase
  \   othercase false ;

[unneeded] date ?\ create date  1 c,  1 c,  2016 ,

  \ doc{
  \
  \ date ( -- a )
  \
  \ Address of a variable that holds the date used by
  \ `set-date` and `get-date`, with the following structure:

  \ ....
  \ +0 day   (1 byte)
  \ +1 month (1 byte)
  \ +2 year  (1 cell)
  \ ....
  \
  \ See: `set-date`, `get-date`.
  \
  \ }doc

[unneeded] get-date ?(

: get-date ( -- day month year )
  date c@ [ date 1+ ] literal c@ [ date 2+ ] literal @ ; ?)

  \ doc{
  \
  \ get-date ( -- day month year )
  \
  \ Get the current date. The default date is 2016-01-01. It
  \ can be changed with `set-date`. The date is not updated by
  \ the system.
  \
  \ See: `set-date`, `date`, `time&date`, `.date`.
  \
  \ }doc

[unneeded] set-date ?(

: set-date ( day month year -- )
  [ date 2+ ] literal ! [ date 1+ ] literal c! date c! ; ?)

  \ doc{
  \
  \ set-date ( day month year -- )
  \
  \ Set the current date. The default date is 2016-01-01. It
  \ can be fetch with `get-date`. The date is not updated by
  \ the system.
  \
  \ See: `get-date`, `date`, `.date`, `leapy-year?`.
  \
  \ }doc

( set-time get-time reset-time )

[unneeded] get-time ?( need ticks need ticks/second

: get-time ( -- second minute hour )
  ticks ticks/second um/mod nip s>d   ( sec . )
                  60 um/mod s>d       ( sec min . )
                  60 um/mod           ( sec min hour ) ; ?)

  \ doc{
  \
  \ get-time ( -- second minute hour )
  \
  \ Return the current time.
  \
  \ NOTE: The computer doesn't have a real clock. The OS frames
  \ counter is used instead, which is increased by the
  \ interrupts routine every 20th ms. The counter is a 24-bit
  \ value, so its maximum is $FFFFFF ticks of 20 ms (5592
  \ minutes, 93 hours), then it starts again from zero.
  \
  \ See also: `set-time`, `time&date`, `.time`.
  \
  \ }doc

[unneeded] set-time ?( need ud* need set-ticks

: set-time ( second minute hour -- )
  3600 um* rot 60 * m+ rot m+ ( seconds )
  ticks/second s>d ud* set-ticks ; ?)

  \ doc{
  \
  \ set-time ( second minute hour -- )
  \
  \ Set the current time.
  \
  \ See also: `get-time`.
  \
  \ }doc

[unneeded] reset-time ?( need reset-ticks need alias
' reset-ticks alias reset-time ( -- ) ?)

  \ doc{
  \
  \ reset-time ( -- )
  \
  \ Reset the current time to 00:00:00.
  \
  \ See also: `get-time`.
  \
  \ }doc

( .00 .0000 .time .system-time .date .system-date )

  \ XXX TODO document

[unneeded] .00 ?\ : .00 ( n -- ) s>d <# # # #> type ;

[unneeded] .0000 ?\ : .0000 ( n -- ) s>d <# # # # # #> type ;

[unneeded] .time ?( need .00

: .time ( second minute hour -- ) .00 ." :" .00 ." :" .00 ; ?)

  \ doc{
  \
  \ .time ( second minute hour -- )
  \
  \ Display the given time in ISO 8601 extended format.
  \
  \ See also: `.date`, `.time&date`, `time&date`.
  \
  \ }doc

[unneeded] .system-time ?( need get-time need .time

: .system-time ( -- ) get-time .time ; ?)

[unneeded] .date ?( need .0000 need .00

: .date ( day month year -- ) .0000 ." -" .00 ." -" .00 ;

  \ doc{
  \
  \ .date ( day month year -- )
  \
  \ Display the given time in ISO 8601 extended format.
  \
  \ See also: `.time`, `.time&date`, `time&date`.
  \
  \ }doc

[unneeded] .system-date ?( need get-date need .date

: .system-date ( -- ) get-date  .date ; ?)

( .time&date time&date )

[unneeded] .time&date ?( need .date need .time

: .time&date ( second minute hour day month year -- )
  .date ." T" .time ; ?)


  \ doc{
  \
  \ .time&date ( second minute hour day month year -- )
  \
  \ Display the given time and date in ISO 8601 extended
  \ format.
  \
  \ See also: `.date`, `.time`, `time&date`.
  \
  \ }doc

[unneeded] time&date ?( need get-time need get-date

: time&date ( -- second minute hour day month year )
  get-time get-date ; ?)

  \ doc{
  \
  \ time&date ( -- second minute hour day month year )
  \
  \ Return the current time and date: second, minute, hour,
  \ day, month and year.
  \
  \ Origin: Forth-94 (FACILITY EXT), Forth-201 (FACILITY EXT).
  \
  \ See also: `get-time`, `get-date`, `set-time`, `set-date`,
  \ `.time&date`.
  \
  \ }doc

( bench{ }bench }bench. bench. benched )

  \ Credit:
  \
  \ Code adapted from Forth Dimensions (volume 17, number 4
  \ page 11, 1995-11).

  \ System-dependent timing routines.

need reset-ticks need ticks need ticks>cs

: bench{ ( -- ) reset-ticks ;

  \ doc{
  \
  \ bench{ ( -- )
  \
  \ Start timing, setting the clock ticks to zero.
  \
  \ See also: `}bench`, `reset-ticks`.
  \
  \ }doc

: }bench ( -- d ) ticks ;

  \ doc{
  \
  \ }bench ( -- d ) ticks ;
  \
  \ Return the current value of the clock ticks.
  \
  \ See also: `bench{`, `ticks`, `bench.`, `}bench.`.
  \
  \ }doc

: bench. ( d -- )
  2dup d. ." ticks ("
  ticks>cs <# # # '.' hold #s #> type ."  s) " ;

  \ doc{
  \
  \ bench. ( d -- )
  \
  \ Display the timing result _d_, which is a number of
  \ clock ticks, in ticks and seconds.
  \
  \ See also: `bench{`, `}bench`, `}bench.`.
  \
  \ }doc

: }bench. ( -- ) ticks bench. ;

  \ doc{
  \
  \ }bench. ( -- )
  \
  \ Stop timing and display the result.
  \
  \ See also: `bench{`, `}bench`, `bench.`.
  \
  \ }doc

: benched ( xt n -- d )
  bench{ 0 ?do  dup execute  loop  }bench rot drop ;

  \ doc{
  \
  \ benched ( xt n -- d )
  \
  \ Execute _n_ times the benchmark _xt_ and return the timer
  \ result _d_.
  \
  \ See also: `bench{`, `}bench`, `benched.`.
  \
  \ }doc

: benched. ( xt n -- )
  bench{ 0 ?do  dup execute  loop  }bench. drop ;

  \ doc{
  \
  \ benched. ( xt n -- d )
  \
  \ Execute _n_ times the benchmark _xt_ and display the
  \ result.
  \
  \ See also: `bench{`, `}bench.`, `benched`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-11-15: Add `leapy-year?`
  \
  \ 2016-05-07: Make block titles compatible with `indexer`.
  \
  \ 2016-05-17: Need `>body`, which has been moved to the
  \ library.
  \
  \ 2016-11-17: Improve documentation of `frames@`, `frames!`,
  \ `reset-frames`.
  \
  \ 2016-12-16: Add `seconds`.
  \
  \ 2016-12-20: Rewrite `ms`, after the v.Forth version.
  \ Rewrite `pause`. Add `do-pause`. Move drafts and variants
  \ of `ms` and `pause` to the development archive, to be
  \ reused in the future.  Rename `jpnext` to `jpnext,` after
  \ the change in the kernel. Update syntax of the `thiscase`
  \ structure.
  \
  \ 2017-01-05: Update `need z80-asm,` to `need assembler`.
  \
  \ 2017-01-17: Fix typo in documentation.
  \
  \ 2017-01-18: Rename `pause` to `?frames` and `do-pause` to
  \ `frames`. The name "pause" was taken from BASIC but the new
  \ names are more clear, and consistent with `ms` and
  \ `seconds`. Write a new `pause` that works exactly like the
  \ homonymous keyword of Sinclair BASIC, as a convenience. Add
  \ `?seconds`. Improve documentation.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-02-03: Compact the code, saving one block. Rename
  \ `(date)` to `date`. Fix `set-date`. Improve documentation.
  \
  \ 2017-02-17: Fix typo in the documentation.  Update cross
  \ references.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-03-16: Make `frames@`, `frames!` and `reset-frames`
  \ individually accessible to `need`.
  \
  \ 2017-03-29: Fix needing of `frames!`. Improve needing of
  \ time and date words. Improve documentation.
  \
  \ 2017-04-27: Move `bench{` and friends here from the
  \ <benchmark.fs>, which is deleted.
  \
  \ 2017-05-06: Add `frames/second`, `frames>seconds`,
  \ `frames>cs`, `frames>ms`.  Improve `bench.` to display
  \ seconds with hundrendths precision. Improve documentation.
  \
  \ 2017-11-27: Improve documentation.
  \
  \ 2017-11-28: Replace the "frames" naming convention with
  \ "ticks". Use `ticks/second` instead of literals.

  \ vim: filetype=soloforth
