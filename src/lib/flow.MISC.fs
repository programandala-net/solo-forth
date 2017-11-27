  \ flow.MISC.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201711271706
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Miscellaneous control flow structures that can be defined
  \ in less than one block.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( +perform base-execute call don't executions )

[unneeded] +perform ( a n -- )
?\ : +perform ( a n -- ) cells + perform ;

  \ doc{
  \
  \ +perform ( a n -- )
  \
  \ Execute the execution token pointed by an offset of _n_
  \ cells from base address _a_, i.e., execute the contents of
  \ element _n_ of the cell table that starts at _a_.
  \
  \ If the execution token is zero, do nothing.
  \
  \ See also: `perform`, `execute`.
  \
  \ }doc

[unneeded] base-execute
?\ : base-execute ( xt n -- ) base @ >r execute r> base ! ;

  \ Credit:
  \
  \ Word from Gforth.

  \ doc{
  \
  \ base-execute ( xt n -- )
  \
  \ Execute _xt_ with the content of `base` being _n_
  \ and restoring the original `base` afterwards.
  \
  \ }doc

[unneeded] call ?(

code call ( a -- )
  E1 c,  C5 c,  CD c, >mark  C1 c,  DD c, 21 c, next , jpnext,
                      >resolve E9 c, end-code ?)

  \   pop hl
  \   push bc
  \   call call_hl
  \   pop bc
  \   ld ix,next
  \   _jp_next
  \ call_hl:
  \   jp (hl)

  \ doc{
  \
  \ call ( a -- )
  \
  \ Call a machine code subroutine at _a_.
  \
  \ }doc

[unneeded] don't ?(

: don't ( n1 n2 -- | n1 n2 )
  2dup = if  2drop unnest unnest  then ; compile-only ?)

  \ doc{
  \
  \ don't ( n1 n2 -- | n1 n2 )
  \
  \ If _n1_ equals _n2_, remove them and exit the definition
  \ that called ``don't``, else leave the _n1_ and _n2_ on the
  \ stack.
  \
  \ ``don't`` is a `compile-only` word.
  \
  \ ``don't`` is intended to be used before `do`, as an
  \ alternative to `?do`, when the do-loop structure is
  \ factored in its own word.
  \
  \ }doc

[unneeded] executions ?( need 2rdrop

  \ Credit:
  \
  \ Code from Galope (module times.fs).

: executions ( xt n -- )
  2>r begin   2r@   while
        2r> 1- 2>r execute  repeat  drop 2rdrop ; ?)

  \ doc{
  \
  \ executions ( xt n -- )
  \
  \ Execute _xt_ _n_ times.
  \
  \ See also: `times`, `dtimes`.
  \
  \ }doc

( ?repeat recurse ?? )

[unneeded] ?repeat ?( need cs-dup

: ?repeat
  \ Compilation: ( dest -- dest )
  \ Run-time:    ( f -- )
  cs-dup  postpone until ; immediate ?)

  \ Credit:
  \
  \ Code from the documentation of Forth-2012 and Forth-94.

  \ Old history:
  \
  \ 2016-03-04: Copy the code of `?repeat` from the Forth-2012
  \ documentation.
  \ 2016-04-28: Fix the stack notation of `?repeat`.
  \ 2016-11-26: Move to <flow.misc.fsb>.

  \ doc{
  \
  \ ?repeat
  \   Compilation: ( dest -- dest )
  \   Run-time:    ( f -- )
  \
  \ An alternative exit point for `begin until` loops.
  \
  \ ``?repeat`` is an `immediate` word.
  \
  \ Usage example:
  \
  \ ----
  \ : xx ( -- )
  \     begin
  \       ...
  \     flag ?repeat  \ Go back to `begin` if flag is false
  \       ...
  \     flag ?repeat  \ Go back to `begin` if flag is false
  \       ...
  \     flag until    \ Go back to `begin` if flag is false
  \     ...
  \ ;
  \ ----

  \ }doc

[unneeded] recurse ?(

: recurse ( -- )
  latestxt compile, ; immediate compile-only ?)

  \ Old history:
  \
  \ 2015-06-05: Written in the kernel.
  \ 2016-04-17: Moved to the library.
  \ 2016-04-24: Fixed with `latestxt`: now it can be used
  \ in words created with `:noname`.
  \ 2016-11-26: Move to <flow.misc.fsb>.

  \ doc{
  \
  \ recurse ( -- )
  \
  \ Append the execution semantics of the current definition to
  \ the current definition.
  \
  \ ``recurse`` is an `immediate` and `compile-only` word.
  \
  \ Origin: Forth-83 (Controlled Reference Words), Forth-94
  \ (CORE), Forth-2012 (CORE).
  \
  \ }doc

[unneeded] ?? ?(

: ?? \ Compilation: ( "name" -- ) Runtime: ( f -- )
  postpone if
  defined ( nt | 0 ) ?dup 0= -13 ?throw
  name>immediate? ( xt f ) if  execute  else  compile,  then
  postpone then
  ; immediate compile-only ?)

  \ Credit:
  \
  \ Original code by Neil Bawd, presented at FORML 1986.

  \ The original code was written two ways:

  \ : ?? \ Compilation: ( "name" -- ) Runtime: ( f -- )
  \   s" if" evaluate  bl word count evaluate
  \   s" then" evaluate
  \ ;  immediate

  \ : ?? \ Compilation: ( "name" -- ) Runtime: ( f -- )
  \   postpone if bl word count evaluate  postpone then
  \ ;  immediate

  \ XXX OLD -- This first version used `postpone` and
  \ `compile,` instead of `evaluate`.

  \ : ?? \ Compilation: ( "name" -- ) Runtime: ( f -- )
  \   postpone if
  \   parse-name find-name 0= -13 ?throw compile,
  \   postpone then
  \ ;  immediate

  \ XXX OLD -- simpler:

  \ : ?? \ Compilation: ( "name" -- ) Runtime: ( f -- )
  \  postpone if  ' compile,  postpone then
  \ ;  immediate

  \ XXX OLD -- even simpler:

  \ : ?? ( f -- ) 0= if  r> cell+ >r  then ; compile-only

  \ Final version, after a comment by Anton Ertl in
  \ comp.lang.forth, 2015-10-19:

  \ XXX TODO -- 2016-11-26: don't compile `if then` for
  \ immediate _name_; compile `drop` instead?

  \ XXX TODO -- 2016-11-26: It seems more useful the old
  \ version, extended as the rest of alternative conditionals:
  \
  \ : ??  ( f -- )  0= if  r> cell+ >r  then ; compile-only
  \ : 0?? ( f -- )     if  r> cell+ >r  then ; compile-only
  \ : -?? ( f -- ) 0>= if  r> cell+ >r  then ; compile-only
  \ : +?? ( f -- )  0< if  r> cell+ >r  then ; compile-only

  \ doc{
  \
  \ ??
  \   Compilation: ( "name" -- )
  \   Run-time:    ( f -- )
  \

  \ ``??`` is an `immediate` and `compile-only` word.
  \
  \ Compilation:
  \
  \ Parse _name_ and search the current search order for it.
  \ If not found, throw exception #-13. If found and it's an
  \ `immediate` word, execute it, else compile it.
  \
  \ Run-time:
  \
  \ If _f_ is not zero, execute _name_, which was compiled.
  \
  \ }doc

( retry ?retry ?leave thens )

  \ Description:
  \
  \ Unconditional and conditional branches to the start of the
  \ current definition.

  \ Credit:

  \ Based on the article "RETRY, EXIT, and Word-Level
  \ Factoring", by Richard Astle, published on Forth Dimensions
  \ (volume 17, number 4, page 19, 1995-11).

  \ Old history:
  \
  \ 2015-11-07: First version.
  \ 2016-04-26: Documented.
  \ 2016-11-26: Move to <flow.misc.fsb>.

[unneeded] retry ?( need name>body

: retry ( -- )
  latest name>body postpone again ; immediate compile-only ?)

  \ doc{
  \
  \ retry ( -- )
  \
  \ Do an unconditional branch to the start of the word.
  \
  \ ``retry`` is an `immediate` and `compile-only` word.
  \
  \ See also: `?retry`.
  \
  \ }doc

[unneeded] ?retry ?( need retry

: ?retry \ Compilation: ( -- ) Run-time: ( f -- )
  postpone if  postpone retry  postpone then
 ; immediate compile-only ?)

  \ doc{
  \
  \ ?retry
  \   \ Compilation: ( -- )
  \   \ Run-time:    ( f -- )
  \
  \ Do a conditional branch to the start of the word.
  \
  \ ``?retry`` is an `immediate` and `compile-only` word.
  \
  \ See also: `retry`.
  \
  \ }doc

[unneeded] ?leave ?(

code ?leave ( f -- ) ( R: loop-sys -- | loop-sys )
  E1 c, 78 04 + c, B0 05 + c, C2 c, ' leave , jpnext,
  \ pop hl
  \ ld a,h
  \ or l
  \ jp nz,leave_
  \ _jp_next
  end-code ?)

  \ doc{
  \
  \ ?leave ( f -- ) ( R: loop-sys -- | loop-sys )
  \
  \ If _f_ is non-zero, discard the loop-control parameters for
  \ the current nesting level and continue execution
  \ immediately following the innermost syntactically enclosing
  \ `loop` or `+loop`.
  \
  \ See also: `leave`, `unloop`, `do`, `?do`.
  \
  \ }doc

( cond thens )

[unneeded] cond ?( need thens

0 constant cond immediate compile-only ?)

  \ doc{
  \
  \ cond
  \   Compilation: ( C: -- 0 )
  \   Run-time:    ( -- )

  \
  \ Compilation: Mark the start of a ``cond`` .. `thens`
  \ structure.  Leave _0_ on the control-flow stack, as a mark
  \ for `thens`.
  \
  \ ``cond`` is an `immediate` and `compile-only` word.
  \
  \ Usage example:

  \ ----
  \ : test ( x -- )
  \   cond
  \     test1 if action1 else
  \     test2 if action2 else
  \     test3 if action3 else
  \     default-action
  \   thens ;
  \ ----

  \ See also: `case`.
  \
  \ }doc

[unneeded] thens ?(

: thens
  \ Compilation: ( C: 0 orig#1 .. orig#n -- )
  \ Run-time:    ( -- )
  begin ?dup while postpone then repeat
  ; immediate compile-only ?)

  \ doc{
  \
  \ thens
  \   Compilation: ( C: 0 orig#1 .. orig#n -- )
  \   Run-time:    ( -- )
  \
  \ Resolve all forward references _orig#1 .. orign#n_ with
  \ `then` until _0_ is found.
  \
  \ ``thens`` is an `immediate` and `compile-only` word.
  \
  \ ``thens`` is a factor of `endcase` and other control
  \ structures, but it's also the end of a `cond` .. ``thens``
  \ structure.

  \ }doc

  \ Credit of the `cond thens` structure:
  \
  \ Subject: Re: Multiple WHILE's
  \ From: Wil Baden <neil...@earthlink.net>
  \ Newsgroups: comp.lang.forth
  \ Message-ID: <260620020959020469%neilbawd@earthlink.net>
  \ Date: Wed, 26 Jun 2002 16:58:18 GMT

  \ ===========================================================
  \ Change log

  \ 2016-11-26: Create this module to combine the modules that
  \ contain small control flow structures, in order to save
  \ blocks: <flow.base-execute.fsb>, <flow.call.fsb>,
  \ <flow.dont.fsb>, <flow.executions.fsb>,
  \ <flow.question-question.fsb>, <flow.question-repeat.fsb>,
  \ <flow.recurse.fsb>, <flow.retry.fsb>.
  \
  \ 2016-11-26: Document `base-execute`.  Compact `call` and
  \ document it. Document `??`. Move `?leave` from the kernel.
  \
  \ 2016-12-03: Fix needing of `retry`.
  \
  \ 2016-12-04: Add `+perform`.
  \
  \ 2016-12-08: Rename the module filename with uppercase
  \ "MISC" after the new convention.
  \
  \ 2016-12-20: Rename `jpnext` to `jpnext,` after the change
  \ in the kernel.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation, after `immediate` or
  \ `compile-only`. Improve `?repeat`. Fix needing of `??`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2017-07-27: Replace `jp next` with the actual macro
  \ `_jp_next` in Z80 comments.
  \
  \ 2017-11-27: Move `cond` and `thens` from <flow.select.fs>
  \ and document them.

  \ vim: filetype=soloforth
