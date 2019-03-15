  \ translation.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201903151737
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Tools to translate programs, i.e. to create and handle text
  \ strings, characters and code in several languages, and use
  \ them automatically depending on the current language
  \ selected by the user.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2017, 2018, 2019.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ ===========================================================

( lang langs localized, )

  \ Words used by `localized-word`, `localized-character`,
  \ `localized-string`, `far-localized-string` and
  \ `far>localized-string`.

unneeding langs ?\ 0 cconstant langs \ number of languages

  \ doc{
  \
  \ langs ( -- b )
  \
  \ A `cconstant` containing the number _b_ of languages used
  \ by the application, needed by translation tools
  \ `localized-word`, `localized-string` and
  \ `localized-character`.
  \
  \ Its default value is zero. The value must be configured by
  \ the application using `c!>`, and it should not be changed
  \ later.
  \
  \ See: `lang`.
  \
  \ }doc

unneeding lang ?\ 0 cconstant lang  \ current language

  \ doc{
  \
  \ lang ( -- b )
  \
  \ A `cconstant` containint the number _b_ of the current
  \ language, used by translation tools `localized-word`,
  \ `localized-string` and `localized-character`.
  \
  \ Its default value is zero. The value must be changed by the
  \ application using `c!>`.
  \
  \ See: `langs`.
  \
  \ }doc

unneeding localized, ?( need langs need n,

: localized, ( x[langs]..x[1] -- ) langs n, ; ?)

unneeding far-localized, ?( need langs need far-n,

: far-localized, ( x[langs]..x[1] -- ) langs ? far-n, ; ?)

( localized-word localized-string localized-character )

unneeding localized-word ?(

need localized, need lang need +perform

: localized-word ( xt[langs]..xt[1] "name" -- )
  create localized,
  does> ( -- ) ( dfa ) lang +perform ; ?)

  \ doc{
  \
  \ localized-word ( xt[langs]..xt[1] "name" -- )
  \
  \ Create a word _name_ that will execute an execution token
  \ from _xt[langs]..xt[1]_, depending on `lang`.
  \ _xt[langs]..xt[1]_, are the execution tokens of the
  \ localized versions.  _xt[langs]..xt[1]_, are ordered by ISO
  \ language code, being TOS the first one.
  \
  \ See: `localized-string`, `localized-character`, `langs`.
  \
  \ }doc

unneeding localized-string ?(

need localized, need lang need array>

: localized-string ( ca[langs]..ca[1] "name" -- )
  create localized,
  \ does> ( -- ca len ) ( dfa ) lang cells + @ count ;
  does> ( -- ca len ) ( dfa ) lang swap array> @ count ; ?)

  \ doc{
  \
  \ localized-string ( ca[langs]..ca[1] "name" -- )
  \
  \ Create a word _name_ that will return a counted string from
  \ _ca[langs]..ca[1]_, depending on `lang`.
  \ _ca[langs]..ca[1]_, are the addresses where the strings
  \ have been compiled.  _ca[langs]..ca[1]_, are ordered by ISO
  \ language code, being TOS the first one.
  \
  \ See: `far-localized-string`, `far>localized-string`,
  \ `localized-word`, `localized-character`, `langs`.
  \
  \ }doc

  \ XXX TODO -- Benchmark `cells +` vs `swap array>`.

unneeding localized-character ?( need langs need lang

: localized-character ( c[langs]..c[1] "name" -- c )
  create langs 0 ?do c, loop
  does> ( -- c ) ( dfa ) lang + c@ ; ?)

  \ doc{
  \
  \ localized-character ( c[langs]..c[1] "name" -- c )
  \
  \ Create a word _name_ that will return a character from
  \ _c[langs]..c[1]_, depending on `lang`.  _c[langs]..c[1]_
  \ are ordered by ISO language code, being TOS the first one.
  \
  \ See: `localized-word`, `localized-string`, `langs`.
  \
  \ }doc

( far-localized-string far>localized-string )

unneeding far-localized-string ?(

need localized, need lang need array>

: far-localized-string ( ca[langs]..ca[1] "name" -- )
  create localized,
  \ does> ( -- ca len ) ( dfa ) lang cells + @ farcount ;
  does> ( -- ca len ) ( dfa ) lang swap array> @ farcount ; ?)

  \ doc{
  \
  \ far-localized-string ( ca[langs]..ca[1] "name" -- )
  \
  \ Create a word _name_ that will return a counted string from
  \ _ca[langs]..ca[1]_, depending on `lang`.
  \
  \ _ca[langs]..ca[1]_, are the far-memory addresses where the
  \ strings have been compiled.  _ca[langs]..ca[1]_, are
  \ ordered by ISO language code, being TOS the first one.
  \
  \ Note the string returned by _name_ is in far memory, where
  \ it's compiled. Therefore the application needs `fartype` or
  \ `far>stringer` to use it.  `far>localized-string` is a
  \ variant of ``far-localized-string`` that returns the
  \ strings already copied in the `stringer`.
  \
  \ See: `far>localized-string`, `localized-string`,
  \ `localized-word`, `localized-character`, `langs`,
  \ `farcount`.
  \
  \ }doc

  \ XXX TODO -- Benchmark `cells +` vs `swap array>`.

unneeding far>localized-string ?(

need localized, need lang need array> need far>stringer

: far>localized-string ( ca[langs]..ca[1] "name" -- )
  create localized,
  \ does> ( -- ca len ) ( dfa ) lang cells + @ farcount >stringer ;
  does> ( -- ca len ) ( dfa )
    lang swap array> @ farcount far>stringer ; ?)

  \ doc{
  \
  \ far>localized-string ( ca[langs]..ca[1] "name" -- )
  \
  \ Create a word _name_ that will return a counted string from
  \ _ca[langs]..ca[1]_, depending on `lang`, and copied in the
  \ `stringer`.
  \
  \ _ca[langs]..ca[1]_, are the far-memory addresses where the
  \ strings have been compiled.  _ca[langs]..ca[1]_, are
  \ ordered by ISO language code, being TOS the first one.
  \
  \ See: `far-localized-string`, `localized-string`,
  \ `localized-word`, `localized-character`, `langs`,
  \ `farcount`, `far>stringer`.
  \
  \ }doc

  \ XXX TODO -- Benchmark `cells +` vs `swap array>`.

  \ ===========================================================
  \ Change log

  \ 2019-03-14: Start. Move the code of `localized-string`,
  \ `localized-word` and `localized-character` from project
  \ _Nuclear Waste Invaders_
  \ (http://programandala.net/en.program.nuclear_waste_invaders.html).
  \
  \ 2019-03-15: Improve documentation and needing. Add
  \ `far-localized-string`, `localized,`,
  \ `far>localized-string`.

  \ vim: filetype=soloforth
