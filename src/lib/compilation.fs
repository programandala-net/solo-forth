  \ compilation.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201704172318
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to compilation.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( [false] [true] [if] [else] [then] )

[unneeded] [true]  ?\  0 constant [false] immediate
[unneeded] [false] ?\ -1 constant [true]  immediate

  \ Note: `[if]` uses 132 bytes of data space (not including
  \ `str=`).

[unneeded] [if] [unneeded] [then] [unneeded] [else] and and ?(

: [else] ( "ccc" -- )
  1 begin  begin  parse-name dup while  2dup s" [if]" str=
                  if    2drop 1+
                  else  2dup s" [else]" str=
                        if    2drop 1- dup if  1+  then
                        else  s" [then]" str= if  1-  then
                        then
                  then  ?dup 0= if exit then
           repeat  2drop
    refill 0= until  drop ; immediate

  \ doc{
  \
  \ [else] ( "ccc" -- )
  \
  \ Parse and discard space-delimited words from the parse
  \ area, including nested occurrences of `[if]`-`[then]`, and
  \ `[if]`-``[else]``-`[then]`, until either the word
  \ ``[else]`` or the  word `[then]` has  been parsed and
  \ discarded. If the  parse area  becomes exhausted, it is
  \ refilled as with `refill`.
  \
  \ }doc

: [if] ( f "ccc" -- ) 0= if postpone [else] then ; immediate

  \ doc{
  \
  \ [if] ( f "ccc" -- )
  \
  \ If _flag_ is true, do nothing. Otherwise, parse and discard
  \ space-delimited words from the parse area, including nested
  \ occurrences of ``[if]``-`[then]`, and
  \ ``[if]``-`[else]`-`[then]`, until either the word `[else]`
  \ or the  word `[then]` has  been parsed and  discarded. If
  \ the  parse area  becomes exhausted, it is refilled as with
  \ `refill`.
  \
  \ ``[if]`` is an `immediate` word.
  \
  \ See also: `?\`, `?(`.
  \
  \ }doc

: [then] ( -- ) ; immediate ?)

  \ doc{
  \
  \ [then] ( -- )
  \
  \ Do nothing. ``[then]`` is parsed and recognized by `[if]`.
  \
  \ ``[then]`` is an `immediate` word.
  \
  \ }doc

( body>name name>body link>name name>link name<name name>name )

[unneeded] body>name
?\ need body> : body>name ( pfa -- nt ) body> >name ;

  \ doc{
  \
  \ body>name ( pfa -- nt )
  \
  \ Get _nt_ from its _pfa_.
  \
  \ See also: `name>body`, `link>name`.
  \
  \ }doc

[unneeded] name>body
?\ need >body : name>body ( nt -- pfa ) name> >body ;

  \ doc{
  \
  \ name>body ( nt -- pfa )
  \
  \ Get _pfa_ from its _nt_.
  \
  \ See also: `body>name`.
  \
  \ }doc

[unneeded] link>name
?\ need alias ' cell+ alias link>name ( nt -- pfa )

  \ doc{
  \
  \ link>name ( lfa -- nt )
  \
  \ Get _nt_ from its _lfa_.
  \
  \ See also: `name>link`.
  \
  \ }doc

[unneeded] name>link
?\ need alias ' cell- alias name>link ( nt -- lfa )

  \ doc{
  \
  \ name>link ( nt -- lfa )
  \
  \ Get _lfa_ from its _nt_.
  \
  \ See also: `link>name`.
  \
  \ }doc

[unneeded] name<name
?\ need name>link : name<name ( nt1 -- nt2 ) name>link far@ ;

  \ doc{
  \
  \ name<name ( nt1 -- nt2 )
  \
  \ Get the previous _nt2_ from _nt1_.
  \
  \ See also: `name>name`.
  \
  \ }doc

[unneeded] name>name
?\ need >>name : name>name ( nt1 -- nt2 ) name>str + >>name ;

  \ doc{
  \
  \ name>name ( nt1 -- nt2 )
  \
  \ Get the next _nt2_ from _nt1_.
  \
  \ See also: `name<name`.
  \
  \ }doc

( >>link name>> >>name >body body> '' [''] )

[unneeded] >>link
?\ need alias  ' cell+ alias >>link ( xtp -- lfa )

[unneeded] name>>
?\ : name>> ( nt -- xtp ) cell- cell- ;

[unneeded] >>name
?\ : >>name ( xtp -- nt ) cell+ cell+ ;

[unneeded] >body
?\ code >body  E1 c, 23 c, 23 c, 23 c, jppushhl, end-code
  \ ( xt -- pfa )
  \ pop hl
  \ inc hl
  \ inc hl
  \ inc hl
  \ jp pushhl

  \ doc{
  \
  \ >body  ( xt -- dfa )
  \
  \ _dfa_ is the data-field address corresponding to _xt_.
  \
  \ If _xt_ is for a word defined by `create`, _dfa_ is the
  \ address that `here` would have returned had it been
  \ executed immediately after the execution of the `create`
  \ that defined _xt_.
  \
  \ If _xt_ is for a word defined by `variable`, `2variable`,
  \ `cvariable`, `constant`, `2constant` and `cconstant`, _dfa_
  \ is the address that holds their value.
  \
  \ If _xt_ is for a word defined by `:`, _dfa_ is the address
  \ of its compiled definition.
  \
  \ If _xt_ is for a word defined by `code`, _dfa_ makes no
  \ sense.
  \
  \ _dfa_ is always in data space.
  \
  \ Origin: Forth-83 (Required Word Set), Forth-94 (CORE),
  \ Forth-2012 (CORE).
  \
  \ See also: `body>`.
  \
  \ }doc

[unneeded] body>
?\ code body> E1 c, 2B c, 2B c, 2B c, jppushhl, end-code
  \ ( pfa -- xt )
  \ pop hl
  \ dec hl
  \ dec hl
  \ dec hl
  \ jp pushhl

  \ doc{
  \
  \ body>  ( dfa -- xt )
  \
  \ _xt_ is the execution token corresponding to the data-field
  \ address _dfa_.
  \
  \ See also: `>body`.
  \
  \ }doc

[unneeded] '' ?( need need-here need-here name>>
: '' ( "name" -- xtp ) defined dup ?defined name>> ; ?)

  \ doc{
  \
  \
  \ '' ( "name" -- xtp )
  \
  \ If _name_ is found in the current search order, return its
  \ execution-token pointer _xtp_, else throw an exception.
  \
  \ Since aliases share the execution token of their original
  \ word, it's not possible to get the name of an alias from
  \ its execution token. But ``''`` can do it:

  \ ----
  \ ' drop alias discard
  \ ' discard >name .name       \ this prints "drop"
  \ '' discard >>name .name     \ this prints "discard"
  \ ----

  \ See also: `['']`, `'`.
  \
  \ }doc

[unneeded] [''] ?( need need-here need-here ''
: ['']  '' postpone literal ; immediate compile-only ?)
  \ ( Compilation: "name" -- )

  \ doc{
  \
  \ [''] ( Compilation: "name" -- )

  \
  \ If _name_ is found in the current search order, compile its
  \ execution-token pointer as a literal, else throw an
  \ exception.
  \
  \ ``['']`` is an `immediate` and `compile-only` word.
  \
  \ See also: `literal`, `''`, `[']`.
  \
  \ }doc

( >name )

need >>name need name>name need name>>

: >name ( xt -- nt | 0 )
  0 begin ( xt xtp )
    dup >>name >r
    far@ over = if  drop r> exit  then
    r> name>name name>>
  np@ over u< until  2drop false ;

  \ 2017-01-20 Note:
  \
  \ Alternative implementation of `>name` in Forth.
  \
  \ Data space used:
  \
  \ -  49 B without requirements
  \ -  78 B with requirements
  \
  \ The Z80 implementation in the kernel uses 94 B
  \ and is 580% faster. See `>name-bench`.

  \ >name ( xt -- nt | 0 )
  \
  \ Try to find the name token _nt_ of the word represented by
  \ execution token _xt_. Return 0 if it fails.
  \
  \ ``>name`` searches all headers, from the oldest one to the
  \ newest one, for the first one whose _xtp_ (execution token
  \ pointer) contains _xt_.  This way, when a word has
  \ additional headers created by `alias` or `synonym`, its
  \ original name is found first.
  \
  \ Origin: Gforth.

( name>interpret name>compile comp' [comp'] )

[unneeded] name>interpret ?(
: name>interpret ( nt -- xt | 0 )
  dup name> swap compile-only? 0= and ; ?)

  \ doc{
  \
  \ name>interpret ( nt -- xt | 0 )
  \
  \ Return _xt_ that represents the interpretation semantics of
  \ the word _nt_. If _nt_ has no interpretation semantics,
  \ return zero.
  \
  \ Origin: Forth-2012 (TOOLS EXT).
  \
  \ }doc

[unneeded] name>compile ?(

: (comp') ( nt -- xt )
  immediate?  if    ['] execute
              else  ['] compile,  then ;

  \ doc{
  \
  \ (comp') ( nt -- xt )
  \
  \ A factor of `name>compile`. If _nt_ is an `immediate` word,
  \ return the _xt_ of `execute`, else return the _xt_ of
  \ `compile,`.
  \
  \ }doc

: name>compile ( nt -- x xt ) dup name> swap (comp') ; ?)

  \ doc{
  \
  \ name>compile ( nt -- x xt )
  \
  \ Compilation token _x xt_ represents the compilation
  \ semantics of the word _nt_. The  returned _xt_ has the
  \ stack effect ( i*x  x -- j*x  ).  Executing _xt_ consumes
  \ _x_ and performs the compilation semantics of the word
  \ represented by _nt_.
  \
  \ Origin: Forth-2012 (TOOLS EXT).
  \
  \ See also: `(comp')`.
  \
  \ }doc

[unneeded] comp' ?( need need-here need-here name>compile

: comp' ( "name" -- x xt )
  defined dup ?defined name>compile ; ?)

  \ doc{
  \
  \ comp' ( "name" -- x xt )
  \
  \ Compilation token _x xt_ represents the compilation
  \ semantics of _name_.
  \
  \ Origin: Gforth.
  \
  \ }doc

[unneeded] [comp'] ?( need need-here need-here comp'

: [comp'] ( Compilation: "name" -- ) ( Run-time: -- x xt )
  comp' postpone 2literal ; immediate compile-only ?)

  \ doc{
  \
  \ [comp'] ( Compilation: "name" -- ) ( Run-time: -- x xt )
  \
  \ Compilation token _x xt_ represents the compilation
  \ semantics of _name_.
  \
  \ ``[comp']`` is an `immediate` and `compile-only` word.
  \
  \ Origin: Gforth.
  \
  \ }doc

( there ?pairs [compile] smudge smudged )

[unneeded] there ?\ : there ( a -- ) dp ! ;

  \ doc{
  \
  \ there ( a -- )
  \
  \ Set _a_ as the address of the data-space pointer.
  \ A non-standard counterpart of `here`.
  \
  \ }doc

[unneeded] ?pairs ?\ : ?pairs ( x1 x2 -- ) <> #-22 ?throw ;

  \ doc{
  \
  \ ?pairs ( x1 x2 -- )
  \
  \ If _x1_ not equals _x2_ throw error #-22 (control structure
  \ mismatch).
  \
  \ }doc

[unneeded] [compile]
?\ : [compile] ( "name" -- ) ' compile, ; immediate

  \ doc{
  \
  \ [compile] ( "name" -- )
  \
  \ Parse _name_. Find _name_. If _name_ has other than default
  \ compilation semantics, append them to the current
  \ definition; otherwise append the execution semantics of of
  \ _name_.
  \
  \ In other words: Force compilation of _name_. This allows
  \ compilation of an `immediate` word when it would otherwise
  \ have been executed.
  \
  \ ``[compile]`` is an `immediate` word.
  \
  \ ``[compile]`` has been be superseded by `postpone`.
  \
  \ Origin: fig-Forth, Forth-79 (Required Word Set), Forth-83
  \ (Required Word Set), Forth-94 (CORE EXT), Forth-2012 (CORE
  \ EXT, obsolescent).
  \
  \ See also: `compile`, `compile,`.
  \
  \ }doc

[unneeded] smudged
?\ : smudged ( nt -- ) dup farc@ smudge-mask xor swap farc! ;

  \ doc{
  \
  \ smudged ( nt -- )
  \
  \ Toggle the "smudge bit" of the given _nt_.
  \
  \ ``smudged`` is obsolete. `hidden` and `revealed` are used
  \ instead.
  \
  \ See also: `smudge`.
  \
  \ }doc

[unneeded] smudge
?\ need smudged  : smudge ( -- ) latest smudged ;

  \ doc{
  \
  \ smudge ( -- )
  \
  \ Toggle the "smudge bit" of the latest definition's name
  \ field.  This prevents an uncompleted definition from being
  \ found during dictionary searches, until compiling is
  \ completed without error.
  \
  \ ``smudge`` is obsolete. `hide` and `reveal` are used
  \ instead.
  \
  \ Origin: fig-Forth.
  \
  \ See also: `smudged`.
  \
  \ }doc

( ]l ]2l ]1l ]cl save-here restore-here )

[unneeded] ]l

?\ : ]l ( x -- ) ] postpone literal ; immediate compile-only

  \ doc{
  \
  \ ]l ( x -- )
  \
  \ A short form of the idiom `] literal`.
  \
  \ ``]l`` is an `immediate` and `compile-only` word.
  \
  \ See also: `]`, `literal`, `]2l`, `]1l`, `]cl`.
  \
  \ }doc

[unneeded] ]2l

?\ : ]2l ( xd -- ) ] postpone 2literal ; immediate compile-only

  \ doc{
  \
  \ ]2l ( xd -- )
  \
  \ A short form of the idiom `] 2literal`.
  \
  \ ``]2l`` is an `immediate` and `compile-only` word.
  \
  \ See also: `]`, `2literal`, `]l`, `]1l`, `]cl`.
  \
  \ }doc

[unneeded] ]1l

?\ : ]1l ( x -- ) ] postpone 1literal ; immediate compile-only

  \ doc{
  \
  \ ]1l ( x -- )
  \
  \ A short form of the idiom `] 1literal`.
  \
  \ ``]1l`` is an `immediate` and `compile-only` word.
  \
  \ See also: `]`, `1literal`, `]2l`, `]l`, `]cl`.
  \
  \ }doc

[unneeded] ]cl

?\ : ]cl ( x -- ) ] postpone cliteral ; immediate compile-only

  \ doc{
  \
  \ ]cl ( x -- )
  \
  \ A short form of the idiom `] cliteral`.
  \
  \ ``]cl`` is an `immediate` and `compile-only` word.
  \
  \ See also: `]`, `cliteral`, `]2l`, `]l`, `]1l`.
  \
  \ }doc

[unneeded] save-here [unneeded] restore-here and ?( need there

variable here-backup
: save-here ( -- ) here here-backup ! ;
: restore-here ( -- ) here-backup @ there ; ?)

  \ XXX TODO -- behead `here-backup`

( possibly exec eval )

  \ Credit:
  \
  \ Code of `possibly` adapted from Wil Baden.

[unneeded] possibly ?(

: possibly ( "name" -- )
  defined ?dup if name> execute then ; ?)

  \ doc{
  \
  \ possibly ( "name" -- )
  \
  \ Parse _name_.  If _name_ is the name of a word in the
  \ current search order, execute it; else do nothing.
  \
  \ }doc

[unneeded] exec ?(

: exec ( "name" -- i*x )
  defined ?dup 0= #-13 ?throw name> execute ; ?)

  \ doc{
  \
  \ exec ( "name" -- i*x )
  \
  \ Parse _name_.  If "name" is the name of a word in the
  \ current search order, execute it; else throw exception
  \ #-13.
  \
  \ }doc

[unneeded] eval ?( need evaluate

: eval ( i*x "name" -- j*x ) parse-name evaluate ; ?)

  \ doc{
  \
  \ eval ( i*x "name" -- j*x )
  \
  \ Parse and evaluate _name_.
  \
  \ This is a common factor of `[const]`, `[2const]` and
  \ `[cconst]`.
  \
  \ }doc

( [const] [2const] [1const] [cconst] )

[unneeded] [const] ?( need eval

: [const] ( "name" -- )
  eval postpone literal ; immediate compile-only ?)

  \ doc{
  \
  \ [const] ( "name" -- )
  \
  \ Evaluate _name_. Then compile the single-cell value left on
  \ the stack.
  \
  \ ``[const]`` is intented to compile constants as literals,
  \ in order to gain execution speed. _name_ can be any word,
  \ as long as its execution returns a single-cell value on the
  \ stack.
  \
  \ Usage example:

  \ ----
  \ 48 constant zx
  \ : test ( -- ) [const] zx . ;
  \ ----

  \ ``[const]`` is an `immediate` and `compile-only` word.
  \
  \ See also: `const`, `[2const]`, `[1const]`, `[cconst]`, `eval`.
  \
  \ }doc

[unneeded] [2const] ?( need eval

: [2const] ( "name" -- )
  eval postpone 2literal ; immediate compile-only ?)

  \ doc{
  \
  \ [2const] ( "name" -- )
  \
  \ Evaluate _name_. Then compile the double-cell value left on
  \ the stack.
  \
  \ ``[2const]`` is intented to compile double-cell constants
  \ as literals, in order to gain execution speed.
  \
  \ Usage example:

  \ ----
  \ 48. 2constant zx
  \ : test ( -- ) [2const] zx d. ;
  \ ----

  \ ``[2const]`` is an `immediate` and `compile-only` word.
  \
  \ See also: `2const`, `[const]`, `[1const]`, `[cconst]`, `eval`.
  \
  \ }doc

[unneeded] [1const] ?( need eval

: [1const] ( "name" -- )
  eval postpone 1literal ; immediate compile-only ?)

  \ doc{
  \
  \ [1const] ( "name" -- )
  \
  \ Evaluate _name_. Then compile the single-cell value left on
  \ the stack, using `1literal`.
  \
  \ ``[1const]`` is intented to compile constants as literals,
  \ when it's uncertain if the literal is a character or a
  \ cell, in order to gain execution speed. _name_ can be any
  \ word, as long as its execution returns a single-cell value
  \ on the stack.
  \
  \ Usage example:

  \ ----
  \ 48 constant zx
  \ : test ( -- ) [1const] zx . ;
  \ ----

  \ ``[1const]`` is an `immediate` and `compile-only` word.
  \
  \ See also: `[2const]`, `[const]`, `[cconst]`, `eval`.
  \
  \ }doc

[unneeded] [cconst] ?( need eval

: [cconst] ( "name" -- )
  eval postpone cliteral ; immediate compile-only ?)

  \ doc{
  \
  \ [cconst] ( "name" -- )
  \
  \ Evaluate _name_. Then compile the char left on the stack.
  \
  \ ``[cconst]`` is intented to compile char constants as
  \ literals, in order to gain execution speed.
  \
  \ Usage example:

  \ ----
  \ 48 cconstant zx
  \ : test ( -- ) [cconst] zx emit ;
  \ ----

  \ ``[cconst]`` is an `immediate` and `compile-only` word.
  \
  \ See also: `cconst`, `[2const]`, `[const]`, `[1const]`, `eval`.
  \
  \ }doc

( warnings ?warn )

need search-wordlist

variable warnings  warnings on

  \ doc{
  \
  \ warnings ( -- a )
  \
  \ User variable that holds a flag. If it's zero, no warning
  \ is shown when a compiled word is not unique in the
  \ compilation word list.  Its default value is _true_.
  \
  \ }doc

: no-warnings? ( -- f ) warnings @ 0= ;

  \ doc{
  \
  \ no-warnings? ( -- f )
  \
  \ Are the warnings deactivated?
  \
  \ See: `?warn`, `warnings`.
  \
  \ }doc

: not-redefined? ( ca len -- ca len xt false | ca len true )
  2dup get-current search-wordlist 0= ;

  \ doc{
  \
  \ not-redefined? ( ca len -- ca len xt false | ca len true )
  \
  \ Is the word name _ca len_ not yet defined in the
  \ compilation word list?
  \
  \ See: `?warn`.
  \
  \ }doc

: ?warn ( ca len -- ca len | ca len xt )
    no-warnings? if unnest exit ( ca len ) then
  not-redefined? if unnest                 then
  ( ca len | ca len xt ) ;

  \ doc{
  \
  \ ?warn ( ca len -- ca len | ca len xt )
  \
  \ Check if a warning about the redefinition of the word name
  \ _ca len_ is needed.  If no warning is needed, unnest the
  \ calling definition and return _ca len_. If a warning is
  \ needed, return _ca len_ and the _xt_ of the word found in
  \ the current compilation wordlist.
  \
  \ ``?warn`` is factor of `warn.throw`, `warn.message` and
  \ `warn-throw`.
  \
  \ See also: `no-warnings?`, `not-redefined?`, `warn.message`,
  \ `warn.throw`, `warn-throw`.
  \
  \ }doc

( warn.throw warn.message warn-throw )

[unneeded] warn.throw ?( need ?warn

: warn.throw ( ca len -- ca len )
  ?warn ( ca len xt ) drop .error-word  #-257 .throw ;

' warn.throw ' warn defer! ?)

  \ doc{
  \
  \ warn.throw ( ca len -- ca len )
  \
  \ Alternative action for the deferred word `warn`.  If the
  \ contents of the user variable `warnings` is not zero and
  \ the word name _ca len_ is already defined in the current
  \ compilation word list, print throw error #-257, without
  \ actually throwing an error.
  \
  \ See: `warnings`, `warn-throw`, `warn.message`, `?warn`.
  \
  \ }doc

[unneeded] warn.message ?( need ?warn

: warn.message ( ca len -- ca len )
  ?warn ( ca len xt ) ." redefined " >name .name ;

' warn.message ' warn defer! ?)

  \ doc{
  \
  \ warn.message ( ca len -- ca len )
  \
  \ Alternative action for the deferred word `warn`.  If the
  \ contents of the user variable `warnings` is not zero and
  \ the word name _ca len_ is already defined in the current
  \ compilation word list, print a warning message.
  \
  \ See: `warnings`, `warn.throw`, `warn-throw`, `?warn`.
  \
  \ }doc

[unneeded] warn-throw ?( need ?warn

: warn-throw ( ca len -- ca len )
  ?warn ( ca len xt ) #-257 throw ;

' warn-throw ' warn defer! ?)

  \ doc{
  \
  \ warn-throw ( ca len -- ca len )
  \
  \ Alternative action for the deferred word `warn`.  If the
  \ contents of the user variable `warnings` is not zero and
  \ the word name _ca len_ is already defined in the current
  \ compilation word list, throw error #-257 instead of
  \ printing a warning message.
  \
  \ See: `warnings`, `warn.throw`, `warn.message`, `?warn`.
  \
  \ }doc

( string-parameter )

  \ Credit:
  \
  \ Inspired by pForth's `param`.

: string-parameter ( -- ca len )
  rp@ cell+ dup >r ( a1 ) ( R: a1 )
    \ get the address, in the return stack,
    \ that contains the return address of the calling word,
    \ which contains the address of the compiled string
  @ count ( ca len ) ( R: a1 )
    \ get the string
  dup char+ r@ @ + ( ca len a2 ) ( R: a1 )
    \ calculate the new return address of the calling word,
    \ in order to skip the string
  r> ! ;
    \ update the return address of the calling word,

  \ XXX TODO -- benchmark this alternative:

: string-parameter2 ( -- ca len )
  rp@ cell+ dup >r ( a1 ) ( R: a1 )
    \ get the address, in the return stack,
    \ that contains the return address of the calling word,
    \ which contains the address of the compiled string
  dup @ count ( a1 ca len ) ( R: a1 )
    \ get the string
  dup char+ rot + ( ca len a2 ) ( R: a1 )
    \ calculate the new return address of the calling word,
    \ in order to skip the string
  r> ! ;
    \ update the return address of the calling word,

: string-parameter3 ( -- ca len )
  \ XXX UNDER DEVELOPMENT
  rp@ cell+ dup ( a1 )
    \ get the address, in the return stack,
    \ that contains the return address of the calling word,
    \ which contains the address of the compiled string
  dup @ count ( a1 ca len )
    \ get the string
  rot dup >r over char+ over + ( ca len a2 )
    \ calculate the new return address of the calling word,
    \ in order to skip the string
  r> ! ;
    \ update the return address of the calling word,

  \ doc{
  \
  \ string-parameter ( -- ca len )
  \
  \ Return a string compiled after the calling word.
  \
  \ See `warning"` and `(warning")` for a usage example.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-06-04: Add `[if] [else] [then]`, adapted from Afera.
  \
  \ 2015-06-17: Add `[true]`, `[false]`.
  \
  \ 2015-06-25: Finish `[if] [else] [then]`.
  \
  \ 2015-10-22: Rename words that convert header addresses.
  \
  \ 2015-10-24: Move `body>name`, `name>link`, `link>name` and
  \ `>>link` from the kernel.
  \
  \ 2015-10-29: Move `smudge` and `smudged` from the kernel.
  \
  \ 2015-11-13: Move `?pairs` from the kernel.
  \
  \ 2016-03-19: Add `save-here` and `restore-here`.
  \
  \ 2016-04-17: Add `name>>`.
  \
  \ 2016-04-24: Add `]l`, `]2l`, `exec`, `eval`.
  \
  \ 2016-04-24: Add `[const]`, `[2const]`, `[cconst]`.
  \
  \ 2016-04-24: Move `cliteral` from the kernel.
  \
  \ 2016-04-24: Move `n,` from module "tool.marker.fsb".
  \
  \ 2016-04-25: Simplify `exec`, move `possibly` from the
  \ module "tool.marker.fsb".
  \
  \ 2016-04-25: Move `n,`, `n@`, `n!` to the module
  \ "memory.misc.fsb".
  \
  \ 2016-04-26: Fix `restore-here`. Add `name>interpret`,
  \ `name>compile`.  Move `current-latest` from the kernel,
  \ formerly called `latest`.
  \
  \ 2016-04-27: Add `comp'`, `[comp']`. Move `warning` from the
  \ kernel. Add `warn.throw`, `warn.message`, `warn-throw` and
  \ common factors.
  \
  \ 2016-04-29: Add `string-parameter`.
  \
  \ 2016-05-02: Join several blocks to save space.
  \
  \ 2016-05-02: Move `[compile]` from the kernel.
  \
  \ 2016-05-04: Compact the blocks.
  \
  \ 2016-05-05: Update `s=` to `str=`.
  \
  \ 2016-05-06: Move `current-latest` back to the kernel.
  \
  \ 2016-05-07: Add `?(`, a simpler alternative to `[if]`.
  \
  \ 2016-05-12: Fix requirements of `[cconst]`.
  \
  \ 2016-05-13: Improve `[else]` with `refill`.
  \
  \ 2016-05-14: Update: `evaluate` has been moved to the
  \ library.
  \
  \ 2016-05-15: Update comment.
  \
  \ 2016-05-17: Move `body>` and `>body` from the kernel.
  \
  \ 2016-05-18: Fix `body>` and `>body`: their codes were
  \ exchanged.
  \
  \ 2016-05-31: Update: `cliteral` has been moved to the
  \ kernel. Add `''` and `>>name`.
  \
  \ 2016-06-01: Move `there` from the kernel. Update. Add
  \ `['']`.
  \
  \ 2016-08-05: Fix requiring of `]l`, `]2l`, `exec` ,`eval`,
  \ `save-here` and `restore-here`. Compact the code of several
  \ blocks. Replace one usage of `[if]` with `?(`.
  \
  \ 2016-11-23: Rename `c!toggle-bits` to `ctoggle`.
  \
  \ 2016-11-24: Make `name<name` compatible with far memory.
  \
  \ 2016-11-26: Improve `name>>` and `>>name`. Try `?warn` and
  \ related words, and improve their documentation. Fix
  \ `warn.throw`.
  \
  \ 2016-12-20: Rename `jppushhl` to `jppushhl,`, after the
  \ change in the kernel.
  \
  \ 2016-12-23: Fix typo `s@`, instead of `@s`.
  \
  \ 2016-12-25: Improve needing of `warn.throw`,
  \ `warn.message`, and `warn-throw`.
  \
  \ 2017-01-05: Remove old system bank support from
  \ `name<name`. Rewrite `smudged`, adapted to far memory.
  \
  \ 2017-01-06: Add missing `exit` after loading some words.
  \ Document `(comp')`.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation, after `immediate` or
  \ `compile-only`, or at the end of a line.
  \
  \ 2017-01-20: Add `name>name`. Add alternative implementation
  \ of `>name` in Forth, a possible replacement for the Z80
  \ version included in the kernel.
  \
  \ 2017-01-23: Improve documentation of `''` and `['']`.
  \
  \ 2017-02-16: Remove `cliteral`, which is in the kernel. Fix
  \ typo in documentation of `eval`.  Deactivate documentation
  \ of the alternative implementation of `>name`.
  \
  \ 2017-02-17: Update notation "behaviour" to "action".
  \ Update cross references.
  \
  \ 2017-02-22: Move `?(` to the kernel.
  \
  \ 2017-02-25: Add `]1l` and `[1const]`. Improve
  \ documentation.
  \
  \ 2017-02-26: Update "hp" notation to "np", after the changes
  \ in the kernel.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-04-16: Improve documentation.
  \
  \ 2017-04-17: Fix and improve documentation. Improve needing
  \ of `[if]`, `[else]`, `[then]`.

  \ vim: filetype=soloforth
