  \ tool.marker.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 202006040214
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

  \ XXX TODO -- test
  \ XXX TODO -- use `save-input` and `restore-source` when
  \ possible

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
  \ created by `marker` and then creating it again.
  \
  \ WARNING: ``anew`` is not fully tested yet.
  \
  \ See: `possibly`.
  \
  \ }doc

  \ XXX TODO -- save and restore also the nt associated (+4):
  \ |===
  \ | +0 | _nt_ of last definition
  \ | +2 | _wid|0_, next wordlist in chain, or zero
  \ | +4 | _nt|0_, word list name pointer, or zero
  \ |===

unneeding wordlists, unneeding @wordlists and ?(

: wordlists, ( -- ) latest-wordlist @
                    begin dup cell- @ ( a nt ) , @ ?dup 0=
                    until ;
  \ XXX TODO -- adapt to `latest-wordlist`

  \ doc{
  \
  \ wordlists, ( -- ) "wordlists-comma"
  \
  \ Store the latest definition of every word list in the data
  \ space, updating `dp`.
  \
  \ ``wordlists,`` is a factor of `marker`.
  \
  \ See: `@wordlists`, `order,`.
  \
  \ }doc

: @wordlists ( a -- ) latest-wordlist @ begin
                        2dup swap @ swap cell- !
                        swap cell+ swap @
                      ?dup 0= until  drop ; ?)
  \ XXX TODO -- adapt to `latest-wordlist`

  \ doc{
  \
  \ @wordlists ( a -- ) "fetch-wordlists"
  \
  \ Fetch the latest definition of every word list from _a_.
  \
  \ ``@wordlists`` is a factor of `unmarker`.
  \
  \ See: `wordlists,`, `latest-wordlist`, `@order`.
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
            @+ latest-wordlist ! @+ set-current
  dup dup @ 1+ cells + >r @order r> @wordlists ;

  \ doc{
  \
  \ unmarker ( a -- )
  \
  \ Set the data-space pointer to _a_ and restore the names
  \ pointer, the latest definition pointers, the word lists
  \ pointer, the compilation word list, the search order and
  \ the configuration of word lists that were saved at _a_ by
  \ `marker,`.
  \
  \ ``unmarker`` is a factor of `marker`.
  \
  \ See: `np!`, `last`, `lastxt`, `latest-wordlist`,
  \ `set-current`, `@order`, `@wordlists`.
  \
  \ }doc

: marker, ( -- )
  np@ , last @ , lastxt @ , latest-wordlist @ ,
  get-current , order, wordlists, ;

  \ doc{
  \
  \ marker, ( -- ) "marker-comma"
  \
  \ Store the names pointer, the latest definition pointers,
  \ the word lists pointer, the current compilation word list,
  \ the search order and the configuration of word lists at the
  \ current data-space pointer, for
  \ later restoration by `unmarker`.
  \
  \ ``marker,`` is a factor of `marker`.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ See: `np@`, `last`, `lastxt`, `latest-wordlist`,
  \ `get-current`, `order,`, `wordlists,`.
  \
  \ }doc

: marker ( "name" -- )
  here marker, create , does> ( -- ) ( dfa ) @ unmarker ;

  \ doc{
  \
  \ marker ( "name" -- )
  \
  \ Create a definition "name". When "name" is executed, it
  \ will restore the data-space pointer, the word lists
  \ pointer, the compilation word list, the search order and
  \ the configuration of word lists to the state they had just
  \ prior to the definition of "name".
  \
  \ WARNING: This word still has some issues. It does not pass
  \ `forth2012-test-suite`.
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

  \ vim: filetype=soloforth
