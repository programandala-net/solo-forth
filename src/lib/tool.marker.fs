  \ tool.marker.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ Last modified: 202006081705
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `marker`, `anew`.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017, 2018,
  \ 2020.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( anew wordlists, @wordlists order, @order )

unneeding anew ?( need possibly need marker

  \ Credit:
  \
  \ Code adapted from Wil Baden.

: anew ( "name" -- ) >in @ possibly >in ! marker ; ?)

  \ doc{
  \
  \ anew ( "name" -- )
  \
  \ Parse _name_. If _name_ is the name of a word in the
  \ current search order, execute it. Then restore `>in` to its
  \ value previous to the parsing of _name_ and execute
  \ `marker`.
  \
  \ The function of ``anew`` is to execute a _name_ already
  \ created by `marker` and then create it again.
  \
  \ See: `possibly`.
  \
  \ }doc

unneeding wordlists, unneeding @wordlists and ?( need @+

: wordlists, ( -- ) last-wordlist @
                    begin @+ , @+ dup >r , @ , r> ?dup 0=
                    until ;

  \ doc{
  \
  \ wordlists, ( -- ) "wordlists-comma"
  \
  \ Store all of the current word lists in the data space,
  \ updating `dp`.
  \
  \ ``wordlists,`` is a factor of `marker,`.
  \
  \ See: `@wordlists`, `order,`, `wordlist`.
  \
  \ }doc

need /wordlist need nup need under+

: @wordlists ( a -- ) last-wordlist @ \ ( from to )
                      begin nup /wordlist move dup cell+ @ ?dup
                      while /wordlist under+
                      repeat drop ; ?)

  \ doc{
  \
  \ @wordlists ( a -- ) "fetch-wordlists"
  \
  \ Fetch the `wordlist` definitions from _a_.
  \
  \ ``@wordlists`` is a factor of `unmarker`.
  \
  \ See: `wordlists,`, `last-wordlist`, `@order`.
  \
  \ }doc

unneeding order, unneeding @order and ?( need nn, need nn@
                                         need get-order

: order, ( -- ) get-order nn, ;

  \ doc{
  \
  \ order, ( -- )
  \
  \ Compile the current search order by executing `get-order`
  \ and `nn,`.
  \
  \ ``order,`` is a useful factor of `marker`.
  \
  \ See: `@order`, `wordlists,`.
  \
  \ }doc

: @order ( a -- ) nn@ set-order ; ?)

  \ doc{
  \
  \ @order ( a -- )
  \
  \ Restore the search order stored at _a_ by executing `nn@`
  \ and `set-order`.
  \
  \ ``@order`` is a useful factor of `unmarker`.
  \
  \ See: `order,`, `@wordlists`.
  \
  \ }doc

( marker )

  \ Credit:
  \
  \ Code partly inspired by m3forth's `marker`:
  \ https://github.com/oco2000/m3forth/blob/master/lib/include/core-ext.f

need @+ need there need np! need order, need @order
need wordlists, need @wordlists

: unmarker ( a -- )
  dup there @+ np! @+ last ! @+ lastxt !
            @+ last-wordlist ! @+ set-current
  dup dup @ 1+ cells + >r @order r> @wordlists ;

  \ doc{
  \
  \ unmarker ( a -- )
  \
  \ Restore the system to the state before the correspondig
  \ `marker` was created. The data that describes the state of
  \ the system was stored at _a_ by `marker,`. The restoration
  \ process is the following:
  \
  \ First set the data-space pointer to _a_ (`there`), then
  \ restore the data stored at _a_: the name-space pointer
  \ (`np!`), the latest definition pointers (`last` and
  \ `lastxt`), the word lists pointer (`last-wordlist`), the
  \ current compilation word list (`set-current`), the search
  \ order (`@order`) and the word lists (`@wordlists`).
  \
  \ ``unmarker`` is a factor of `marker`.
  \
  \ }doc

: marker, ( -- )
  np@ , latest , latestxt , last-wordlist @ ,
  get-current , order, wordlists, ;

  \ doc{
  \
  \ marker, ( -- ) "marker-comma"
  \
  \ Save the current state of the system before creating the
  \ corresponding `marker` that will restore it with
  \ `unmarker`. The data that describes the state of the system
  \ is stored at the current data-space pointer (`here`), while
  \ the data-space pointer itself is stored in the body of the
  \ new `marker`. The saving process is the following:
  \
  \ Store at the current data-space pointer the names pointer
  \ (`np@`), the latest definition pointers (`latest` and
  \ `latestxt`), the word lists pointer (`last-wordlist`),
  \ the current compilation word list (`get-current`), the
  \ search order (`order,`) and the word lists (`wordlists,`)
  \ at the current data-space pointer.
  \
  \ ``marker,`` is a factor of `marker`.
  \
  \ }doc

: marker ( "name" -- )
  here marker, create , does> ( -- ) ( dfa ) @ unmarker ;

  \ doc{
  \
  \ marker ( "name" -- )
  \
  \ Create a definition _name_. When _name_ is executed, it
  \ will restore all dictionary allocation and search order
  \ pointers to the state they had just prior to the definition
  \ of "name". Remove the definition of _name_ and all
  \ subsequent definitions.
  \
  \ The following data are preserved and restored: the
  \ data-space pointer (`here`), the name-space pointer
  \ (`np@`), the word lists pointer (`last-wordlist`), the
  \ compilation word list (`get-current`), the search order
  \ (`order`) and the word lists (`dump-wordlists`).
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ See: `marker,`, `unmarker`, `anew`.
  \
  \ }doc

  \ ===========================================================
  \ Change log

  \ 2015-10-27: Add `possibly`, `anew`.
  \
  \ 2015..2016: Drafts of `marker`.
  \
  \ 2016-01-01: Add example of `marker` from m3forth.
  \
  \ 2016-04-24: Move `n,` to module "compilation.fsb".
  \
  \ 2016-04-25: First working version of `marker`. Move
  \ `possibly` to the module "compilation.fsb".
  \
  \ 2016-06-01: Update: `there` was moved from the kernel to
  \ the library.
  \
  \ 2016-11-13: Rename "np" to "hp" after the changes in the
  \ kernel.
  \
  \ 2017-01-06: Update `voc-link` to `latest-wordlist`;
  \ `wordlists,` and `@wordlists` still must be adapted.
  \
  \ 2017-02-26: Update "hp" notation to "np", after the changes
  \ in the kernel.
  \
  \ 2017-03-13: Improve documentation.
  \
  \ 2017-09-09: Update notation "pfa" to the standard "dfa".
  \
  \ 2018-03-09: Add words' pronunciaton.
  \
  \ 2018-03-11: Fix: replace old `@cell+` with `@+`. Compact
  \ the code, saving one block.
  \
  \ 2020-05-18: Update: `np!` has been moved to the library.
  \
  \ 2020-06-03: Improve documentation. Update source style.
  \ Make `order,`, `@order`, `wordlists,` and `@wordlists`
  \ independent.
  \
  \ 2020-06-07: Finish/fix/test `marker` and its factors: now
  \ `marker` passes the Forth-2012 Test Suite. Test `anew`.
  \ Improve documentation.
  \
  \ 2020-06-08: Update: rename `latest-wordlist` to
  \ `last-wordlist`.

  \ vim: filetype=soloforth
