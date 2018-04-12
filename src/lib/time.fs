  \ time.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201804121315
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to time.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( seconds ?seconds ms )

unneeding seconds ?\ need ms : seconds ( u -- ) 1000 * ms ;

  \ doc{
  \
  \ seconds ( u -- )
  \
  \ Wait at least _u_ seconds.
  \
  \ See: `?seconds`, `ms`, `ticks`.
  \
  \ }doc

unneeding ?seconds ?( need ?ticks-pause need ticks/second

: ?seconds ( u -- ) ticks/second * ?ticks-pause ; ?)

  \ doc{
  \
  \ ?seconds ( u -- ) "question-seconds"
  \
  \ Wait at least _u_ seconds or until a key is pressed.
  \
  \ See: `seconds`, `ms`, `?ticks-pause`.
  \
  \ }doc

unneeding ms ?( need assembler

code ms ( u -- )
  d pop, d tstp, nz? rif
    rbegin #171 a ld#, rbegin nop, a dec, z? runtil
           d decp, d tstp,
    z? runtil
  rthen jpnext, end-code ?)

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
  \ See: `seconds`, `ticks-pause`.
  \
  \ }doc

( ticks dticks set-ticks set-dticks reset-ticks reset-dticks )

unneeding ticks ?( need os-frames

: ticks ( -- u ) os-frames @ ; ?)

  \ doc{
  \
  \ ticks ( -- n )
  \
  \ Return the current count of clock ticks _n_, which is
  \ updated by the OS.
  \
  \ NOTE: ``ticks`` returns the low 16 bits of the OS frames
  \ counter, which is increased by the OS interrupts routine
  \ every 20th ms. The counter is actually a 24-bit value,
  \ which can be fetched by `dticks`.
  \
  \ Origin: Comus.
  \
  \ See: `set-ticks`, `reset-ticks`, `ticks/second`,
  \ `ticks>seconds`, `ms>ticks`, `os-frames`, `bench{`.
  \
  \ }doc

unneeding dticks ?( need os-frames

: dticks ( -- ud )
  os-frames @ [ os-frames cell+ ] literal c@ ; ?)

  \ doc{
  \
  \ dticks ( -- ud ) "d-ticks"
  \
  \ Return the current count of clock ticks _ud_, which is
  \ updated by the OS.
  \
  \ NOTE: ``dticks``returns the OS frames counter, which is
  \ increased by the OS interrupts routine every 20th ms. The
  \ counter is a 24-bit value.
  \
  \ See: `ticks`, `set-dticks`, `reset-dticks`,
  \ `ticks/second`, `dticks>seconds`, `bench{`.
  \
  \ }doc

unneeding set-ticks ?( need os-frames

: set-ticks ( n -- ) os-frames ! ; ?)

  \ doc{
  \
  \ set-ticks ( d -- )
  \
  \ Set the system clock to _n_ ticks.
  \
  \ See: `set-dticks`, `ticks`, `reset-ticks`,
  \ `ticks/second`, `bench{`.
  \
  \ }doc

unneeding set-dticks ?( need os-frames

: set-dticks ( d -- )
  [ os-frames cell+ ] literal c! os-frames ! ; ?)

  \ doc{
  \
  \ set-dticks ( d -- ) "set-d-ticks"
  \
  \ Set the system clock to _d_ ticks.
  \
  \ See: `set-ticks`, `dticks`, `reset-dticks`,
  \ `ticks/second`, `bench{`.
  \
  \ }doc

unneeding reset-ticks

?\ need set-ticks : reset-ticks ( -- ) 0 set-ticks ;

  \ doc{
  \
  \ reset-ticks ( -- )
  \
  \ Reset the low 16 bits of the OS clock to zero ticks.
  \
  \ See: `ticks`, `set-dticks`, `ticks/second`, `bench{`.
  \
  \ }doc

unneeding reset-dticks

?\ need set-dticks : reset-dticks ( -- ) 0. set-dticks ;

  \ doc{
  \
  \ reset-dticks ( -- ) "reset-d-ticks"
  \
  \ Reset the system clock to zero ticks.
  \
  \ See: `reset-ticks`, `dticks`, `set-dticks`, `ticks/second`,
  \ `bench{`.
  \
  \ }doc

( ms/tick ticks/second dticks>cs dticks>ms dticks>seconds )

unneeding ms/tick ?\ 20 cconstant ms/tick
  \ XXX TODO -- Calculate after the lina Forth system.

  \ doc{
  \
  \ ms/tick ( -- n ) "ms-slash-tick"
  \
  \ Return the duration _n_ of one clock tick in miliseconds.
  \
  \ See: ticsk/second`, `ticks`.
  \
  \ }doc

unneeding ticks/second ?( need ms/tick

1000 ms/tick / cconstant ticks/second ?)

  \ doc{
  \
  \ ticks/second ( -- n ) "ticks-slash-second"
  \
  \ Return the number _n_ of clock ticks per second.
  \
  \ See: `ms/tick`, `dticks>seconds`, `dticks>cs`,
  \ `dticks>ms`, `ticks`.
  \
  \ }doc

unneeding dticks>cs ?( need ms/tick need d*

: dticks>cs ( d1 -- d2 ) [ ms/tick 10 / s>d ] 2literal d* ; ?)

  \ doc{
  \
  \ dticks>cs ( d1 -- d2 ) "d-ticks-to-cs"
  \
  \ Convert clock ticks _d1_ to centiseconds _d2_.
  \
  \ See: `ticks>cs`, `dticks>seconds`, `dticks>ms`,
  \ `ticks/second`, `ticks`.
  \
  \ }doc

unneeding dticks>ms ?( need ms/tick need d*

: dticks>ms ( d1 -- d2 ) [ ms/tick s>d ] 2literal d* ; ?)

  \ doc{
  \
  \ dticks>ms ( d1 -- d2 ) "d-ticks-to-ms"
  \
  \ Convert clock ticks _d1_ to milliseconds _d2_.
  \
  \ See: `ticks>ms`, `dticks>seconds`, `dticks>cs`,
  \ `ticks/second`, `ticks`.
  \
  \ }doc

unneeding dticks>seconds ?( need ticks/second need m/

: dticks>seconds ( d -- n ) ticks/second m/ nip ; ?)

  \ doc{
  \
  \ dticks>seconds ( d -- n ) "d-ticks-to-seconds"
  \
  \ Convert clock ticks _d_ to seconds _n_.
  \
  \ See: `ticks>seconds`, `dticks>cs`, `dticks>ms`,
  \ `ticks/second`, `ticks`.
  \
  \ }doc

( ms>ticks elapsed delapsed timer dtimer past? dpast? )

unneeding ms>ticks ?( need ms/tick

: ms>ticks ( n1 -- n2 ) ms/tick / ; ?)

  \ doc{
  \
  \ ms>ticks ( n1 -- n2 ) "ms-to-ticks"
  \
  \ Convert _n1_ milisecnods to the corresponding number _n2_
  \ of `ticks`.
  \
  \ See: `ms/tick`.
  \
  \ }doc

unneeding elapsed ?( need ticks

: elapsed ( u1 -- u2 ) ticks swap - ; ?)

  \ doc{
  \
  \ elapsed ( u1 -- u2 )
  \
  \ For the time _u1_ in `ticks` return the elapsed time _u2_
  \ since then, also in `ticks`.
  \
  \ See: `timer`, `delapsed`, `ticks>seconds`, `ticks>cs`,
  \ `ticks>ms`.
  \
  \ }doc

unneeding delapsed ?( need dticks

: delapsed ( d1 -- d2 ) dnegate dticks d+ ; ?)

  \ doc{
  \
  \ delapsed ( d1 -- d2 ) "d-elapsed"
  \
  \ For the time _d1_ in `dticks` return the elapsed time _d2_
  \ since then, also in `dticks`.
  \
  \ See: `dtimer`, `elapsed`, `dticks>seconds`, `dticks>cs`,
  \ `dticks>ms`.
  \
  \ }doc

unneeding timer ?\ need elapsed : timer ( u -- ) elapsed u. ;

  \ doc{
  \
  \ timer ( u -- )
  \
  \ For the time _u_ in `ticks` display the elapsed time since
  \ then, also in `ticks`.
  \
  \ Origin: Comus.
  \
  \ See: `dtimer`, `elapsed`, `ticks>seconds`, `ticks>cs`,
  \ `ticks>ms`.
  \
  \ }doc

unneeding dtimer

?\ need delapsed need ud. : dtimer ( d -- ) delapsed ud. ;

  \ doc{
  \
  \ dtimer ( d -- ) "d-timer"
  \
  \ For the time _d_ in `dticks` display the elapsed time
  \ since then, also in `dticks`.
  \
  \ See: `timer`, `delapsed`.
  \
  \ }doc

unneeding past? ?\ need ticks : past? ( u -- f ) ticks u< ;

  \ doc{
  \
  \ past? ( u -- f ) "past-question"
  \
  \ Return true if the `ticks` clock has passed _u_.
  \
  \ Usage example: The following word will execute the
  \ hypothetical word ``test`` for _u_ clock `ticks`:

  \ ----
  \ : try ( u -- ) ticks + begin test dup past? until drop ;
  \ ----

  \ Origin: lina.
  \
  \ See: `dpast?`, `elapsed`, `timer`.
  \
  \ }doc

unneeding dpast? ?( need dticks need d0<

: dpast? ( d -- f ) dnegate dticks d+ d0< 0= ; ?)

  \ XXX REMARK --  As of 2017-12-12, the following
  \ implementation is slower (see `past?-bench`):
  \
  \ : dpast? ( ud -- f ) dticks du< ;

  \ doc{
  \
  \ dpast? ( ud -- f ) "d-past-question"
  \
  \ Return true if the `dticks` clock has passed _ud_.
  \
  \ Usage example: The following word will execute the
  \ hypothetical word ``test`` for _ud_ clock `dticks`:

  \ ----
  \ : dtry ( ud -- )
  \   dticks + begin test 2dup dpast? until 2drop ;
  \ ----

  \ Origin: lina's ``past?``.
  \
  \ See: `past?`, `delapsed`, `dtimer`.
  \
  \ }doc

( ticks>cs ticks>ms ticks>seconds )

unneeding ticks>cs ?( need ms/tick

: ticks>cs ( n1 -- n2 ) [ ms/tick 10 / ] xliteral * ; ?)

  \ doc{
  \
  \ ticks>cs ( n1 -- n2 ) "ticks-to-cs"
  \
  \ Convert clock `ticks` _n1_ to centiseconds _n2_.
  \
  \ See: `dticks>cs`, `ticks>seconds`, `ticks>ms`,
  \ `ticks/second`.
  \
  \ }doc

unneeding ticks>ms ?( need ms/tick

: ticks>ms ( n1 -- n2 ) ms/tick / ; ?)

  \ doc{
  \
  \ ticks>ms ( n1 -- n2 ) "ticks-to-ms"
  \
  \ Convert clock ticks _n1_ to milliseconds _n2_.
  \
  \ See: `ms>ticks`, `dticks>ms`, `ticks>seconds`, `ticks>cs`,
  \ `ticks/second`, `ticks`.
  \
  \ }doc

unneeding ticks>seconds ?( need ticks/second

: ticks>seconds ( n1 -- n2 ) ticks/second / ; ?)

  \ doc{
  \
  \ ticks>seconds ( n1 -- n2 ) "ticks-to-seconds"
  \
  \ Convert clock ticks _n1_ to seconds _n2_.
  \
  \ See: `dticks>seconds`, `ticks>cs`, `ticks>ms`,
  \ `ticks/second`, `ticks`.
  \
  \ }doc

( ?ticks-pause ticks-pause basic-pause )

unneeding ?ticks-pause ?( need ticks

: ?ticks-pause ( u -- )
  ticks + begin dup ticks u< key? dup if key drop then
                                  or until drop ; ?)

  \ XXX TODO -- multitasking

  \ doc{
  \
  \ ?ticks-pause ( u -- ) "question-ticks-pause"
  \
  \ Stop execution during at least _u_ clock ticks, or until a
  \ key is pressed.
  \
  \ See: `ticks-pause`, `basic-pause`, `?seconds`,
  \ `ticks/second`.
  \
  \ }doc

unneeding ticks-pause ?( need ticks

: ticks-pause ( u -- )
  ticks + begin dup ticks u< until drop ; ?)

  \ XXX TODO -- multitasking

  \ doc{
  \
  \ ticks-pause ( u -- )
  \
  \ Stop execution during at least _u_ clock ticks.
  \
  \ See: `?ticks-pause`, `basic-pause`, `seconds`, `ms`,
  \ `ticks/second`.
  \
  \ }doc

  \ Alternative definition in assembler
  \
  \ XXX TODO -- rewrite with Z80 opcodes

  \ need assembler

  \ code ticks-pause ( u -- )
  \   d pop, b push,
  \   rbegin halt, d decp, d tstp, z? runtil
  \   b pop, jpnext, end-code ?)
  \ XXX FIXME -- `0 ticks-pause` does `$FFFF ticks-pause`

  \ Credit:
  \
  \ Code adapted from Spectrum Forth-83.

  \ XXX TODO -- multitasking

unneeding basic-pause ?( need ?ticks-pause need new-key

: basic-pause ( u -- )
  ?dup if ?ticks-pause else new-key drop then ; ?)

  \ doc{
  \
  \ basic-pause ( u -- )
  \
  \ If _u_ is zero, stop execution until a key is pressed.
  \ Otherwise stop execution during at least _u_ clock `ticks`,
  \ or until a key is pressed.
  \
  \ ``basic-pause`` is a convenience that works like Sinclair
  \ BASIC's ``PAUSE``.
  \
  \ See: `ticks-pause`, `?ticks-pause`, `?seconds`,
  \ `ticks/second`.
  \
  \ }doc

( leapy-year? date set-date get-date )

unneeding leapy-year? ?(

: leapy-year? ( n -- f )
  dup 400 mod 0= if drop true  exit then
  dup 100 mod 0= if drop false exit then
        4 mod 0= if      false exit then false ; ?)

  \ Credit:
  \
  \ Code written by Wil Baden, published on Forth Dimensions
  \ (volume 8, number 5, page 31, 1987-01).

  \ doc{
  \
  \ leapy-year? ( n -- f ) "leapy-year-question"
  \
  \ Is _n_ a leapy year?
  \
  \ See: `set-date`.
  \
  \ }doc

  \ Alternative implementation:

  \ need thiscase

  \ : leapy-year? ( n -- f )
  \   thiscase 400 mod 0= ifcase true  exitcase
  \   thiscase 100 mod 0= ifcase false exitcase
  \   thiscase   4 mod 0= ifcase true  exitcase
  \   othercase false ;

unneeding date ?\ create date 1 c, 1 c, 2016 ,

  \ doc{
  \
  \ date ( -- a )
  \
  \ _a_ is the address of a 3-cell table containing the date
  \ used by `set-date` and `get-date`, with the following
  \ structure:

  \ ....
  \ +0 day   (1 byte)
  \ +1 month (1 byte)
  \ +2 year  (1 cell)
  \ ....
  \
  \ See: `set-date`, `get-date`.
  \
  \ }doc

unneeding get-date ?(

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

unneeding set-date ?(

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

unneeding get-time ?( need ticks need ticks/second

: get-time ( -- second minute hour )
  ticks ticks/second um/mod nip s>d ( sec . )
                  60 um/mod s>d     ( sec min . )
                  60 um/mod         ( sec min hour ) ; ?)

  \ doc{
  \
  \ get-time ( -- second minute hour )
  \
  \ Return the current time.
  \
  \ NOTE: The computer doesn't have a real clock. The OS frames
  \ counter is used instead, which is increased by the OS
  \ interrupts routine every 20th ms. The counter is a 24-bit
  \ value, so its maximum is $FFFFFF ticks of 20 ms (335544
  \ seconds or 5592 minutes or 93 hours), then it starts again
  \ from zero.
  \
  \ See: `set-time`, `time&date`, `.time`.
  \
  \ }doc

unneeding set-time ?( need ud* need set-ticks

: set-time ( second minute hour -- )
  3600 um* rot 60 * m+ rot m+ ( seconds )
  [ ticks/second s>d ] 2literal ud* set-ticks ; ?)

  \ doc{
  \
  \ set-time ( second minute hour -- )
  \
  \ Set the current time.
  \
  \ See: `get-time`.
  \
  \ }doc

unneeding reset-time ?( need reset-ticks need alias

' reset-ticks alias reset-time ( -- ) ?)

  \ doc{
  \
  \ reset-time ( -- )
  \
  \ Reset the current time to 00:00:00.
  \
  \ See: `get-time`.
  \
  \ }doc

( .time .date .time&date time&date )

unneeding .time ?( need .00

: .time ( second minute hour -- ) .00 ." :" .00 ." :" .00 ; ?)

  \ doc{
  \
  \ .time ( second minute hour -- ) "dot-time"
  \
  \ Display the given time in ISO 8601 extended format.
  \
  \ See: `.date`, `.time&date`, `time&date`, `.00`.
  \
  \ }doc

unneeding .date ?( need .0000 need .00

: .date ( day month year -- ) .0000 ." -" .00 ." -" .00 ;

  \ doc{
  \
  \ .date ( day month year -- ) "dot-date"
  \
  \ Display the given time in ISO 8601 extended format.
  \
  \ See: `.time`, `.time&date`, `time&date`, `.0000`, `.00`.
  \
  \ }doc

unneeding .time&date ?( need .date need .time

: .time&date ( second minute hour day month year -- )
  .date ." T" .time ; ?)

  \ doc{
  \
  \ .time&date ( second minute hour day month year -- ) "dot-time-and-date"
  \
  \ Display the given time and date in ISO 8601 extended
  \ format.
  \
  \ See: `.date`, `.time`, `time&date`.
  \
  \ }doc

unneeding time&date ?( need get-time need get-date

: time&date ( -- second minute hour day month year )
  get-time get-date ; ?)

  \ doc{
  \
  \ time&date ( -- second minute hour day month year ) "time-and-date"
  \
  \ Return the current time and date: second, minute, hour,
  \ day, month and year.
  \
  \ Origin: Forth-94 (FACILITY EXT), Forth-201 (FACILITY EXT).
  \
  \ See: `get-time`, `get-date`, `set-time`, `set-date`,
  \ `.time&date`.
  \
  \ }doc

( bench{ }bench }bench. bench. benched )

  \ Credit:
  \
  \ Code adapted from Forth Dimensions (volume 17, number 4
  \ page 11, 1995-11).

  \ System-dependent timing routines.

need reset-dticks need dticks need dticks>cs

: bench{ ( -- ) reset-dticks ;

  \ doc{
  \
  \ bench{ ( -- ) "bench-curly-bracket"
  \
  \ Start timing, setting the clock ticks to zero.
  \
  \ See: `}bench`, `reset-dticks`.
  \
  \ }doc

: }bench ( -- d ) dticks ;

  \ doc{
  \
  \ }bench ( -- d ) "curly-bracket-bench"
  \
  \ Return the current value of the clock ticks.
  \
  \ See: `bench{`, `dticks`, `bench.`, `}bench.`.
  \
  \ }doc

: bench. ( d -- )
  2dup d. ." ticks ("
  dticks>cs <# # # '.' hold #s #> type ."  s) " ;

  \ doc{
  \
  \ bench. ( d -- ) "bench-dot"
  \
  \ Display the timing result _d_, which is a number of
  \ clock ticks, in ticks and seconds.
  \
  \ See: `bench{`, `}bench`, `}bench.`.
  \
  \ }doc

: }bench. ( -- ) dticks bench. ;

  \ doc{
  \
  \ }bench. ( -- ) "curly-bracket-bench-dot"
  \
  \ Stop timing and display the result.
  \
  \ See: `bench{`, `}bench`, `bench.`.
  \
  \ }doc

: benched ( xt n -- d )
  bench{ 0 ?do dup execute loop }bench rot drop ;

  \ doc{
  \
  \ benched ( xt n -- d )
  \
  \ Execute _n_ times the benchmark _xt_ and return the timer
  \ result _d_.
  \
  \ See: `bench{`, `}bench`, `benched.`.
  \
  \ }doc

: benched. ( xt n -- )
  bench{ 0 ?do dup execute loop }bench. drop ;

  \ doc{
  \
  \ benched. ( xt n -- d ) "benched-dot"
  \
  \ Execute _n_ times the benchmark _xt_ and display the
  \ result.
  \
  \ See: `bench{`, `}bench.`, `benched`.
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
  \ "ticks". Use `ticks/second` instead of literals. Rename
  \ `pause` `basic-pause` and fix it. Improve documentation.
  \ Fix `?ticks-pause` with `key`. Add 1-cell versions of
  \ "tick" words and rename all of them accordingly.
  \
  \ 2017-12-03: Improve conversions between ticks and time
  \ units.
  \
  \ 2017-12-04: Add `elapsed`, `delapsed`, `timer` and
  \ `dtimer`.  Fix and improve documentation. Make `dticks>ms`
  \ and `set-time` faster. Add `expired` and `dexpired`.
  \
  \ 2017-12-09: Improve documentation.
  \
  \ 2017-12-10: Move `.00` and `.0000` to <display.numbers.fs>.
  \ Improve documentation. Remove `.system-time` and
  \ `.system-date`.
  \
  \ 2017-12-12: Rename `expired` `past?`, and `dexpired`
  \ `dpast?`. Add `ms>ticks`.
  \
  \ 2018-01-03: Update `1literal` to `xliteral`.
  \
  \ 2018-03-05: Update `[unneeded]` to `unneeding`.
  \
  \ 2018-03-08: Add words' pronunciaton.
  \
  \ 2018-04-09: Fix stack comment. Update source layout (remove
  \ double spaces). Fix and improve documentation.
  \
  \ 2018-04-12: Fix link in documentation.

  \ vim: filetype=soloforth
