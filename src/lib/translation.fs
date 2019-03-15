  \ translation.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201903151606
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

  \ Words used by `localized-word`, `localized-string` and
  \ `localized-character`.

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

( localized-word localized-string localized-character )

unneeding localized-word ?(

need localized, need lang need +perform

: localized-word ( xt[langs]..xt[1] "name" -- )
  create localized,
  does> ( -- ) ( pfa ) lang +perform ; ?)

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
  \ does> ( -- ca len ) ( pfa ) lang cells + @ count ;
  does> ( -- ca len ) ( pfa ) lang swap array> @ count ; ?)

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
  \ See: `localized-word`, `localized-character`, `langs`.
  \
  \ }doc

  \ XXX TODO -- Benchmark `cells +` vs `swap array>`.

unneeding localized-string ?( need langs need lang

: localized-character ( c[langs]..c[1] "name" -- c )
  create langs 0 ?do c, loop
  does> ( -- c ) ( pfa ) lang + c@ ; ?)

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

  \ ===========================================================
  \ Change log

  \ 2019-03-14: Start. Move the code of `localized-string`,
  \ `localized-word` and `localized-character` from project
  \ _Nuclear Waste Invaders_
  \ (http://programandala.net/en.program.nuclear_waste_invaders.html).
  \
  \ 2019-03-15: Improve documentation and needing.

  \ vim: filetype=soloforth
