  \ modules.package.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 201804152100
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ Implementation of SwiftForth's packages, which are reusable
  \ named modules with private and public definitions.

  \ ===========================================================
  \ Authors

  \ Julian Fondren, 2016.

  \ Marcos Cruz (programandala.net) adapted the code to Solo
  \ Forth, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

  \ ===========================================================
  \ References

  \ Newsgroups: comp.lang.forth
  \ Date: Tue, 2 Aug 2016 04:00:38 -0700 (PDT)
  \ Message-ID: <0a8d7b8a-8367-4e92-a482-ee8b6728325a@googlegroups.com>
  \ Subject: Code management with wordlists
  \ From: Julian Fondren
  \
  \ http://forth.minimaltype.com/packages.html

  \ https://www.forth.com/swiftforth/

( package public private end-package )

  \ ............................................
  \ XXX REMARK -- 2016-12-07:

  \     Data space used:
  \
  \       Code              76 B
  \       Requirements    2073 B (!)

  \ The problem is `-order`, which needs `n>r`, which needs the
  \ assembler.

  \ ............................................

need latest>wordlist need +order need -order need nextname

: package ( "name" -- wid0 wid1 )
  get-current parse-name find-name ?dup
  if    name> execute
  else  wordlist dup parsed-name 2@ nextname constant
                 dup latest>wordlist
  then  dup set-current  dup +order ;

  \ doc{

  \ package ( "name" -- wid0 wid1 )
  \
  \ If the package _name_ has been previously defined, open it.
  \ Otherwise create it.
  \
  \ _wid1_ is the word list of the package _name_; _wid0_
  \ is the word list in which the package _name_ was created.
  \
  \ `end-package` ends the package; `public` start public
  \ definitions and `private` starts private definitions.
  \
  \ Syntax:

  \ ----
  \ package package-name
  \ ... private definitions here ...
  \ public
  \ ... public definitions here ...
  \ private
  \ ... more private definitions maybe ...
  \ end-package
  \ ----

  \ In the above, private definitions are placed in the
  \ ``package-name`` word list.  Public definitions are placed
  \ in whatever word list was current before ``package
  \ package-name``.  If a package called ``package-name``
  \ already exists prior to the above, then it is reused,
  \ rather than redefined.

  \ Usage example:
  \
  \ ----
  \ package example
  \
  \ defer text
  \ : ex1 ( -- ca len ) s" This is an example" ;
  \ ' ex1 ' text defer!
  \
  \ public
  \
  \ : .example ( -- ) text cr type ;
  \
  \ private
  \
  \ : ex2 ( -- ca len ) s" This is an example (cont.)" ;
  \
  \ end-package
  \ ----

  \ At this point, ``.example`` is a new word in whatever the
  \ current wordlist was, and ``text``, ``ex1`` and ``ex2`` are
  \ all words in the ``example`` word list. ``example`` itself
  \ is created in the current wordlist if it didn't already
  \ exist.  (if ``example`` exists and *isn't* a package, this
  \ is an unchecked error which will probably be revealed when
  \ `public` runs.)
  \
  \ If this code is in a library, code including the library
  \ can then run ``.example`` freely.
  \
  \ If there's some need to reopen the package, this is easily
  \ done:

  \ ----
  \ package example
  \
  \ :noname ( -- ca len ) s" This is yet another example" ; '
  \ text defer!
  \
  \ end-package
  \
  \ .example
  \ ----

  \ Use case: loading a package using library with a prelude:
  \
  \ Suppose that you need to load a package with some alien
  \ definitions, you can put them in a package with the same
  \ name before loading the code, and this will only affect
  \ that package:

  \ ----
  \ package some-package
  \
  \ \ This package's code relies on ``place`` appending a nul byte.
  \
  \ : place ( ca1 len1 ca2 -- ) 2dup + 0 swap c!  place ;
  \
  \ end-package
  \
  \ include some-lib.fs
  \ ----

  \ Origin: SwiftForth.
  \
  \ See: `internal`, `isolate`, `module`, `privatize`,
  \ `seclusion`.
  \
  \ }doc

: public ( wid0 wid1 -- wid0 wid1 ) over set-current ;

  \ doc{
  \
  \ public ( wid0 wid1 -- wid0 wid1 )
  \
  \ Mark subsequent definitions available outside the current
  \ package defined with `package`.
  \
  \ _wid1_ is the word list of the current package; _wid0_
  \ is the word list in which the current package was created.
  \
  \ Origin: SwiftForth.
  \
  \ See: `end-package`, `private`.
  \
  \ }doc

: private ( wid0 wid1 -- wid0 wid1 ) dup set-current ;

  \ doc{
  \
  \ private ( wid0 wid1 -- wid0 wid1 )
  \
  \ Mark subsequent definitions invisible outside the current
  \ package. This is the default condition following the usage
  \ of `package`.
  \
  \ _wid1_ is the word list of the current package; _wid0_
  \ is the word list in which the current package was created.
  \
  \ Origin: SwiftForth.
  \
  \ See: `end-package`, `public`.
  \
  \ }doc

: end-package ( wid0 wid1 -- ) -order set-current ;

  \ doc{
  \
  \ end-package ( wid0 wid1 -- )
  \
  \ End the current package, which was started by `package`.
  \
  \ _wid1_ is the word list of the current package; _wid0_
  \ is the word list in which the current package was created.
  \
  \ Origin: SwiftForth.
  \
  \ See: `public`, `private`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2016-12-06: Start.
  \
  \ 2016-12-07: Fix `package`, test everything, complete
  \ documentation, calculate the data space used.
  \
  \ 2017-01-07: Rename `named-wid` to `latest>wordlist`, after
  \ the changes in the word-lists interface words.
  \
  \ 2017-02-17: Update cross references.  Change markup of
  \ inline code that is not a cross reference.
  \
  \ 2017-03-14: Improve documentation.
  \
  \ 2018-04-15: Fix markup in documentation.

  \ vim: filetype=soloforth
