  \ tool.marker.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 201709091154
  \ See change log at the end of the file

  \ ===========================================================
  \ Description

  \ `marker`, `anew`.

  \ ===========================================================
  \ Author

  \ Marcos Cruz (programandala.net), 2015, 2016, 2017.

  \ ===========================================================
  \ License

  \ You may do whatever you want with this work, so long as you
  \ retain every copyright, credit and authorship notice, and
  \ this license.  There is no warranty.

( marker )

  \ XXX TODO -- save and restore also the nt associated (+4):
  \ |===
  \ | +0 | _nt_ of last definition
  \ | +2 | _wid|0_, next wordlist in chain, or zero
  \ | +4 | _nt|0_, word list name pointer, or zero
  \ |===

: wordlists, ( -- )
  latest-wordlist @ begin
    dup cell- @ ( a nt ) , @
  ?dup 0= until ;
  \ XXX TODO -- adapt to `latest-wordlist`

  \ doc{
  \
  \ wordlists, ( -- )
  \
  \ Store the latest definition of every word list in the data
  \ space.
  \
  \ }doc

: @wordlists ( a -- )
  latest-wordlist @ begin
    2dup  swap @ swap cell- !
    swap cell+ swap  @
  ?dup 0= until  drop ;
  \ XXX TODO -- adapt to `latest-wordlist`

  \ doc{
  \
  \ @wordlists ( a -- )
  \
  \ Fetch the latest definition of every word list from _a_.
  \
  \ }doc

-->

( marker )

  \ Credit:
  \
  \ Code partly inspired by m3forth's `marker`:
  \ https://github.com/oco2000/m3forth/blob/master/lib/include/core-ext.f

need get-order need @cell+ need nn, need nn@ need there

: @order ( a -- ) nn@ set-order ;

: unmarker ( a -- )
  dup there
  @cell+ np!  @cell+ last !  @cell+ lastxt !
  @cell+ latest-wordlist !
  @cell+ set-current
  dup dup @ 1+ cells + >r  @order  r> @wordlists ;

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
  \ }doc

: order, ( -- ) get-order nn, ;

: marker, ( -- a )
  here  np@ ,  last @ ,  lastxt @ ,  latest-wordlist @ ,
        get-current ,  order,  wordlists, ;

  \ doc{
  \
  \ marker, ( -- a )
  \
  \ Store the names pointer, the latest definition pointers,
  \ the word lists pointer, the current compilation word list,
  \ the search order and the configuration of word lists at the
  \ current data-space pointer, and return its address _a_, for
  \ later restoration by `unmarker`.
  \
  \ ``marker,`` is a factor of `marker`.
  \
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ }doc

: marker ( "name" -- )
  marker, create ,  does> ( -- ) ( dfa ) @ unmarker ;

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
  \ Origin: Forth-94 (CORE EXT), Forth-2012 (CORE EXT).
  \
  \ }doc

( anew )

need possibly need marker

  \ Credit:
  \
  \ Code adapted from Wil Baden.

  \ XXX TODO -- test
  \ XXX TODO -- use `save-input` and `restore-source` when
  \ possible

: anew ( "name" -- ) >in @  possibly  >in !  marker ;

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

  \ vim: filetype=soloforth
