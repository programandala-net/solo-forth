  \ strings.escaped.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201712092318
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Words related to escaped strings.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( esc-standard-chars-wordlist )

get-current  forth-wordlist set-current need parse-char

wordlist dup constant esc-standard-chars-wordlist
dup >order set-current case-sensitive @ case-sensitive on

  \ doc{
  \
  \ esc-standard-chars-wordlist ( -- wid )
  \
  \ Identifier of the word list that contains the words whose
  \ names are the standard characters that must be escaped
  \ after a backslash in strings parsed by `s\"`, `.\"` and
  \ other words.
  \
  \ The execution of the words defined in the word list
  \ identified by ``esc-standard-chars-wordlist`` returns the
  \ new character(s) on the stack (the last one at the bottom)
  \ and the count. Example of the stack effect of a escaped
  \ character that returns two characters:

  \ ----
  \   ( -- c[1] c[0] 2 )
  \ ----

  \ Most of the escaped characters are translated to one
  \ character, so they are defined as double constants.
  \
  \ Conversion rules:

  \ [cols="8,8,16"]
  \ |===
  \ | Escaped | Name | ASCII characters
  \
  \ | \a | BEL (alert)          |  7
  \ | \b | BS (backspace)       |  8
  \ | \e | ESC (escape)         | 27
  \ | \f | FF (form feed)       | 12
  \ | \l | LF (line feed)       | 10
  \ | \m | CR/LF                | 13, 10
  \ | \n | newline              | 13
  \ | \q | double-quote         | 34
  \ | \r | CR (carriage return) | 13
  \ | \t | HT (horizontal tab)  |  9
  \ | \v | VT (vertical tab)    | 11
  \ | \z | NUL (no character)   |  0
  \ | \" | double-quote         | 34
  \ | \x<hexdigit><hexdigit>  | | Conversion of the two hexadecimal digits
  \ |===

  \ See: `parse-esc-string`, `set-esc-order`,
  \ `esc-standard-chars-wordlist`, `esc-block-chars-wordlist`,
  \ `esc-udg-chars-ẁordlist`.
  \
  \ }doc

7 1 2constant a  8 1 2constant b  27 1 2constant e
  \ \a = backspace
  \ \b = alert
  \ \e = escape
12 1 2constant f  10 1 2constant l  '"' 1 2constant q
  \ \f = form feed
  \ \l = line feed
  \ \q = double quote
13 1 2constant r  9 1 2constant t  11 1 2constant v
  \ \r = carriage return
  \ \t = horizontal tab
  \ \v = vertical tab
0 1 2constant z  '\' 1 2constant \
  \ \z = null character
  \ \\ = backslash

: m ( -- c1 c2 2 ) 10 13 2 ;
  \ \m = carriage return and line feed

: (x) ( "c" -- n ) parse-char 16 digit? 0= #-260 ?throw ;
  \ Parse an hex digit and convert it to a number.

: x ( "<hexdigit><hexdigit>" -- c 1 ) (x) 16 * (x) + 1 ;
  \ \x = hex character code
  \ Parse the 8-bit hex number of a character code.

'"' 1 2constant "  13 1 2constant n
  \ \" = double quote
  \ \n = new line

case-sensitive ! set-current previous

( parse-esc-string )

need parse-esc-char>chars need chars>string need s+
need get-esc-order need catch

: (parse-esc-string) ( ca len "ccc<quote>"  -- ca' len' )
  begin   parse-char dup '"' <>  \ not finished?
  while   dup '\' =  \ maybe escaped?
          if    drop parse-esc-char>chars
          else  1  then  chars>string s+
  repeat  drop ;

  \ doc{
  \
  \ (parse-esc-string) ( ca len "ccc<quote>"  -- ca' len' )
  \
  \ Parse a text string delimited by a double quote,
  \ translating some configurable characters that are escaped
  \ with a backslash.  Add the translated string to _ca len_,
  \ returning a new string _ca' len'_ in the `stringer`.
  \
  \ ``(parse-esc-string)`` is a factor of `parse-esc-string`.
  \
  \ See: `set-esc-order`.
  \
  \ }doc

variable case-sensitive-esc-chars  case-sensitive-esc-chars on

  \ doc{
  \
  \ case-sensitive-esc-chars ( -- a )
  \
  \ A variable. _a_ is the address of a cell containing a flag
  \ that turns case-sensitive mode on and off only during the
  \ parsing of escaped strings, e.g.  `s\"` and `.\"`.  The
  \ contents of this variable are temporarily stored into
  \ `case-sensitive` by `parse-esc-string`. The current
  \ contents of `case-sensitive` are preserved and restored at
  \ the end.
  \
  \ When the contents of `case-sensitive` are non-zero, escaped
  \ characters case-sensitive mode is on (this is the default):
  \ any escaped character searched for in the configured word
  \ list will not be modified, therefore it will be found only
  \ if it's identical to the name stored in the definition
  \ header.
  \
  \ When the contents of `case-sensitive-esc-chars` are zero,
  \ escaped characters case-sensitive mode is off: any escaped
  \ character searched for in the correspondent word list will
  \ be converted to lowercase first (the conversion is done at
  \ low level, not affecting the name string passed as
  \ parameter).
  \
  \ NOTE: In order to create upper-case case-sensitive escaped
  \ chars, their correspondent words must be created when
  \ `case-sensitive` is on.  See the words defined in
  \ `esc-udg-chars-wordlist`.
  \
  \ }doc

: parse-esc-string ( "ccc<quote>"  -- ca len )
  get-order get-esc-order set-order
  case-sensitive @ case-sensitive-esc-chars @ case-sensitive !
  0 0 ['] (parse-esc-string) catch >r
  2>r case-sensitive ! set-order 2r> r> throw ;

  \ doc{
  \
  \ parse-esc-string ( "ccc<quote>"  -- ca len )
  \
  \ Parse a text string delimited by a double quote,
  \ translating some configurable characters that are escaped
  \ with a backslash.  Return the translated string _ca len_ in
  \ the `stringer`.
  \
  \ The characters that must be escaped depend on the search
  \ order set by the deferred word `set-esc-string-order`,
  \ whose default action is `set-standard-esc-string-order`.
  \ Therefore, by default, the escaped characters are those
  \ described in Forth-2012's `s\"`.
  \
  \ ``parse-esc-string`` is a common factor of `s\"` and `.\"`.
  \
  \ See: `(parse-esc-string)`.
  \
  \ }doc

( s\" .\" )

[unneeded] s\" ?(

need parse-esc-string need esc-standard-chars-wordlist

need set-esc-order  esc-standard-chars-wordlist 1 set-esc-order

: s\"
  \ Interpretation: ( "ccc<quote>" -- ca len )
  \ Compilation:    ( "ccc<quote>" -- )
  \ Run-time:       ( -- ca len )
  parse-esc-string compiling? if  postpone sliteral  then
 ; immediate ?)

  \ XXX TODO -- Finish the documentation.

  \ doc{
  \
  \ s\"
  \   Compilation:    ( "ccc<quote>" -- )
  \   Interpretation: ( "ccc<quote>" -- ca len )
  \   Run-time:       ( -- ca len )

  \
  \ Note: When ``s\"`` is loaded, `esc-standard-chars-wordlist`
  \ is set as the only word list in `esc-order`. That is the
  \ standard behaviour. Alternative escaped chars can be
  \ configured with `esc-block-chars-wordlist` and
  \ `esc-udg-chars-wordlist`.
  \
  \ ``s\"`` is an `immediate` word.
  \
  \ Origin: Forth-2012 (CORE EXT, FILE EXT).
  \
  \ See: `parse-esc-string`, `set-esc-order`, `.\"`.
  \
  \ }doc

[unneeded] .\" ?(

need parse-esc-string need esc-standard-chars-wordlist

need set-esc-order  esc-standard-chars-wordlist 1 set-esc-order

: .\"
  \ Compilation: ( "ccc<quote>" -- )
  \ Run-time:    ( -- ca len )
  compile (.") parse-esc-string s, ; immediate compile-only ?)

  \ XXX TODO documentation

  \ doc{
  \
  \ .\"
  \   Compilation: ( "ccc<quote>" -- )
  \   Run-time:    (-- ca len )
  \

  \ ``.\"`` is an `immediate` and `compile-only` word.
  \
  \ Note: When ``.\"`` is loaded, `esc-standard-chars-wordlist`
  \ is set as the only word list in `esc-order`. That is the
  \ standard behaviour. Alternative escaped chars can be
  \ configured with `esc-block-chars-wordlist` and
  \ `esc-udg-chars-wordlist`.
  \
  \ See: `parse-esc-string`, `set-esc-order`, `s\"`.
  \
  \ }doc

( max-esc-order #esc-order esc-context ?esc-order )

[unneeded] max-esc-order
?\ 4 constant max-esc-order

  \ doc{
  \
  \ max-esc-order ( -- n )
  \
  \ A constant that returns the maximum number of word lists in
  \ the escaped strings search order.
  \
  \ Its default value is 4, but the application can define this
  \ constant with any other value before loading the words that
  \ need it, and it will be kept.
  \
  \ See: `esc-context`, `#esc-order`, `set-esc-order`,
  \ `get-esc-order`, `>order`.
  \
  \ }doc

[unneeded] #esc-order [unneeded] esc-context and ?(

need max-esc-order

variable #esc-order  #esc-order off

  \ doc{
  \
  \ #esc-order ( -- a )
  \
  \ A variable. _a_ is the address of a cell containing the
  \ number of word lists in the escaped strings search order.
  \
  \ See: `esc-context`, `esc-max-order`, `esc-get-order`,
  \ `esc-set-order`, `>esc-order`.
  \
  \ }doc

create esc-context here max-esc-order cells dup allot erase ?)

  \ doc{
  \
  \ esc-context ( -- a )
  \
  \ A variable that holds the escaped strings search order: _a_
  \ is the address of an array of cells, whose maximum length
  \ is hold in the `max-esc-order` constant, and whose current
  \ length is hold in the `#esc-order` variable.  _a_ holds the
  \ word list at the top of the search order.
  \
  \ See: `esc-max-order`, `>esc-order`, `esc-get-order`,
  \ `esc-set-order`.
  \
  \ }doc

[unneeded] ?esc-order ?(

need max-esc-order

: ?esc-order ( n -- )
  dup 0< #-282 ?throw  max-esc-order < ?exit  #-281 throw ; ?)

  \ doc{
  \
  \ ?esc-order ( n -- )
  \
  \ Check if _n_ is a valid size for the escaped strings search
  \ order, else throw an exception.
  \
  \ See: `#esc-order`, `esc-context`, `>esc-order`,
  \ `set-esc-order`, `get-esc-order`.
  \
  \ }doc

( set-esc-order get-esc-order )

[unneeded] set-esc-order ?(

need ?esc-order need #esc-order need esc-context

: set-esc-order ( widn..wid1 n -- )
  dup ?esc-order  dup #esc-order !
  0 ?do  i cells esc-context + !  loop ; ?)

  \ doc{
  \
  \ esc-set-order ( widn..wid1 n -- )
  \
  \ Set the escaped strings search order to the word lists
  \ identified by _widn..wid1_. Subsequently, word list _wid1_
  \ will be searched first, and word list _widn_ searched last.
  \ If _n_ is zero, empty the escaped strings search order.
  \
  \ See: `esc-get-order`, `>esc-order`,
  \ `esc-standard-chars-wordlist`, `esc-block-chars-wordlist`,
  \ `esc-udg-chars-ẁordlist`.
  \
  \ }doc

[unneeded] get-esc-order ?(

need #esc-order need esc-context

: get-esc-order ( -- wid[n]..wid[1] n )
  #esc-order @ 0 ?do
    #esc-order @ i - 1- cells esc-context + @
  loop  #esc-order @ ; ?)

  \ doc{
  \
  \ get-esc-order ( -- wid[n]..wid[1] n )
  \
  \ Return the number of word lists _n_ in the escaped strings
  \ search order and the word lists identifiers
  \ _wid[n]..wid[1]_ identifying these word lists.  _wid[1]_
  \ identifies the word list that is searched first, and
  \ _wid[n]_ the word list that is searched last.
  \
  \ See: `set-esc-order`, `>esc-order`.
  \
  \ }doc

( >esc-order parse-esc-char>chars )

[unneeded] >esc-order ?(

need get-esc-order need set-esc-order

: >esc-order ( wid -- )
  >r get-esc-order 1+ r> swap set-esc-order ; ?)

  \ doc{
  \
  \ >esc-order ( wid -- )
  \
  \ Push _wid_ on the escaped strings search order.
  \
  \ See: `set-esc-order`, `get-esc-order`,
  \ `esc-standard-chars-wordlist`,
  \ `esc-block-chars-wordlist`,
  \ `esc-udg-chars-ẁordlist`.
  \
  \ }doc

[unneeded] esc-previous ?(
: esc-previous ( -- )
  get-esc-order nip 1- set-esc-order ; ?)

  \ doc{
  \
  \ esc-previous ( -- )
  \
  \ Remove the top word list (the word list that is searched
  \ first) from the escaped strings search order.
  \
  \ }doc

[unneeded] parse-esc-char>chars ?(

need parse-char need char>string

: parse-esc-char>chars ( "c" -- c[n-1]..c[0] n )
  parse-char dup char>string find-name
  ?dup if  nip name> execute  else  '\' 2  then ; ?)

  \ doc{
  \
  \ parse-esc-char>chars ( "c" -- c[n-1]..c[0] n )
  \
  \ Parse and translate a escaped char 'c' to a number of chars
  \ _c[n-1]..c[0] and their count _n_.
  \
  \ The translation is done by searching the name of the
  \ escaped char in the current search order, which has been
  \ set by calling `set-esc-string-order` in
  \ `parse-esc-string`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-04-24: Remove `[char]` and `char`, which have been
  \ moved to the library.
  \
  \ 2016-05-18: Use `wordlist` instead of `vocabulary`, which
  \ has been moved to the library.
  \
  \ 2016-12-18: Improve documentation of `escaped-wordlist`.
  \
  \ 2016-12-22: Rename `escaped-wordlist` to
  \ `esc-standard-chars-wordlist`.  Make the escaped characters
  \ configurable with the search order. Improve documentation.
  \ Rename "escaped" to "esc" in word names, because some of
  \ them were longer than 31 characters. Rename `unescape-char`
  \ to `esc-char>chars` and fix it. Factor `parse-esc-string`.
  \ Use `alias` (31 bytes) if already defined, to save 14 bytes
  \ of 2 double-cell constants defined in
  \ `esc-standard-chars-wordlist`.  Add `parse-char` to
  \ `esc-char>chars`, rename it `parse-esc-char>chars` and make
  \ it accessible to `need`.
  \
  \ 2016-12-23: Add support for case-sensitive escaped chars.
  \ Improve the management of the escaped strings search order:
  \ Instead of a deferred word, use a set of words to
  \ manipulate the escaped strings search order, analogous to
  \ the main search order.
  \
  \ 2017-01-06: Fix erasing of `esc-context`: the parameters
  \ were wrong and the Forth system crashed or became
  \ corrupted, depending on the size of the dictionary and the
  \ previous words.
  \
  \ 2017-01-11: Make `s\"` and `.\"` use
  \ `esc-standard-chars-wordlist`. Improve documentation.
  \
  \ 2017-01-18: Remove `exit` at the end of conditional
  \ interpretation.
  \
  \ 2017-01-19: Remove remaining `exit` at the end of
  \ conditional interpretation, after `immediate` or
  \ `compile-only`.
  \
  \ 2017-01-27: Remove remaining `exit` after definition of
  \ `max-esc-order`. Improve needing of `?esc-order`. Fix
  \ `parse-esc-string`: execute `(parse-esc-string)` with
  \ `catch` in order to restore the search order before
  \ actually throwing a possible exception (e.g. thrown by
  \ `(x)`); otherwise the system is unusable, because the
  \ search order is the one used to parse the escaped strings.
  \
  \ 2017-02-01: Remove unnecessary `upper` from `(x)`.
  \
  \ 2017-02-17: Update cross references.
  \
  \ 2017-02-27: Improve documentation.
  \
  \ 2017-03-12: Update mentions to the `stringer`.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-05-05: Improve documentation.
  \
  \ 2017-05-07: Improve documentation.
  \
  \ 2017-12-09: Don't use `alias` as an alternative to define
  \ `"` and `n`, since `[defined]` and `[undefined]` are moved
  \ to the library.
  \
  \ 2017-12-09: Improve documentation.

  \ vim: filetype=soloforth
