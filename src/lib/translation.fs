  \ translation.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201903141723
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

need n,

0 cconstant langs \ number of languages

0 cconstant lang  \ current language

: localized, ( x[langs]..x[1] -- ) langs n, ;

( localized-word localized-string localized-character )

unneeding localized-word ?(

need localized, need lang need +perform

: localized-word ( xt[langs]..xt[1] "name" -- )
  create localized,
  does> ( -- ) ( pfa ) lang +perform ; ?)

  \ Create a word _name_ that will execute an execution token
  \ from _xt[langs]..xt[1]_, depending on the current language.
  \ _xt[langs]..xt[1]_, are the execution tokens of the
  \ localized versions.  _xt[langs]..xt[1]_, are ordered by ISO
  \ language code, being TOS the first one.

unneeding localized-string ?(

need localized, need lang need array>

: localized-string ( ca[langs]..ca[1] "name" -- )
  create localized,
  \ does> ( -- ca len ) ( pfa ) lang cells + @ count ;
  does> ( -- ca len ) ( pfa ) lang swap array> @ count ; ?)

  \ Create a word _name_ that will return a counted string
  \ from _ca[langs]..ca[1]_, depending on the current language.
  \ _ca[langs]..ca[1]_, are the addresses where the localized
  \ strings have been compiled.  _ca[langs]..ca[1]_, are
  \ ordered by ISO language code, being TOS the first one.
  \
  \ XXX TODO -- Benchmark `cells +` vs `swap array>`.

unneeding localized-string ?( need langs need lang

: localized-character ( c[langs]..c[1] "name" -- c )
  create langs 0 ?do c, loop
  does> ( -- c ) ( pfa ) lang + c@ ; ?)

  \ Create a word _name_ that will return a character
  \ from _c[langs]..c[1]_, depending on the current language.
  \ _c[langs]..c[1]_ are ordered by ISO language code, being
  \ TOS the first one.

  \ ===========================================================
  \ Change log

  \ 2019-03-14: Start. Move the code of `localized-string`,
  \ `localized-word` and `localized-character` from project
  \ _Nuclear Waste Invaders_
  \ (http://programandala.net/en.program.nuclear_waste_invaders.html).

  \ vim: filetype=soloforth
