  \ prog.editor.gforth.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 202005112053
  \ See change log at the end of the file

  \ ===========================================================
  \ Authors

  \ Bernd Paysan, 1995.
  \
  \ Adapted to Solo Forth by Marcos Cruz (programandala.net),
  \ 2016, 2017, 2018.

  \ ===========================================================
  \ Description

  \ This is the simple block editor included with Gforth (in
  \ blocks file <blocked.fb>), adapted to Solo Forth.
  \
  \ Word descriptions and stack comments have been added
  \ following the original source.

( gforth-editor )

only forth definitions

need inverse need list need update
need list-lines need vocabulary need catch need editor
need insert need replace need delete

vocabulary gforth-editor ' gforth-editor is editor

  \ doc{
  \
  \ gforth-editor ( -- )
  \
  \ A `vocabulary` containing a port of the Gforth block
  \ editor. When ``gforth-editor`` is loaded, it becomes the
  \ action of `editor`.

  \ .Gforth block editor commands
  \ |===
  \ | Word | Description
  \

  \ | `<<src-lib-prog-editor-gforth-fs, a>>`
  \ ``( -- )``
  \ | Go to marked position.

  \ | `<<src-lib-prog-editor-gforth-fs, c>>`
  \ ``( n -- )``
  \ | Move cursor by _n_ chars.

  \ | `<<src-lib-prog-editor-gforth-fs, d>>`
  \ ``( -- )``
  \ | Delete marked area.

  \ | `dl`
  \ ``( -- )``
  \ | Delete a line at the cursor position.

  \ | `<<src-lib-prog-editor-gforth-fs, f>>`
  \ ``( "ccc<eol>" -- )``
  \ | Search _ccc_ and mark it.

  \ | `<<src-lib-prog-editor-gforth-fs, g>>`
  \ ``( u -- )``
  \ | Go to screen _u_.

  \ | `<<src-lib-prog-editor-gforth-fs, i>>`
  \ ``( "ccc<eol>" -- )``
  \ | Insert _ccc_; if _ccc_ is empty, instert the contents of
  \ the insert buffer.

  \ | `il`
  \ ``( -- )``
  \ | Insert a line at the cursor position..

  \ | `<<src-lib-prog-editor-gforth-fs, l>>`
  \ ``( -- )``
  \ | List current screen.

  \ | `<<src-lib-prog-editor-gforth-fs, m>>`
  \ ``( -- )``
  \ | Mark current position.

  \ | `<<src-lib-prog-editor-gforth-fs, n>>`
  \ ``( -- )``
  \ | Go to next screen.

  \ | `<<src-lib-prog-editor-gforth-fs, p>>`
  \ ``( -- )``
  \ | Go to previous screen.

  \ | `<<src-lib-prog-editor-gforth-fs, r>>`
  \ ``( "ccc<eol>" -- )``
  \ | Replace marked area.

  \ | `<<src-lib-prog-editor-gforth-fs, s>>`
  \ ``( u "ccc<eol>" -- )``
  \ | Search _ccc_ until screen _u_; if _ccc_ is empty, use the
  \ string of the previous search.

  \ | `<<src-lib-prog-editor-gforth-fs, t>>`
  \ ``( u "ccc<eol>"-- )``
  \ | Go to line _u_ and insert _ccc_.

  \ | `<<src-lib-prog-editor-gforth-fs, y>>`
  \ ``( -- )``
  \ | Yank deleted string.
  \ |===
  \
  \ See: `specforth-editor`.
  \
  \ }doc

also editor definitions  need r# need top

variable len len off
  \ Length of the text found.
  \ XXX TODO -- rename to `/r#`
  \ XXX TODO -- move to <prog.editor.COMMON.fsb>

2variable mark 0. mark 2!
  \ Backup of the editing position (cursor and block).

-->

( gforth-editor )

create rbuf $100 allot
  \ XXX TODO -- rename
  \ XXX TODO -- use the `stringer` instead?

  \ doc{
  \
  \ rbuf ( -- ca )
  \
  \ Return the address _ca_ of the 100-byte replace buffer used
  \ by the `gforth-editor`.
  \
  \ See: `ibuf`, `fbuf`.
  \
  \ }doc

create ibuf $100 allot
  \ XXX TODO -- rename
  \ XXX TODO -- use the `stringer` instead?

  \ doc{
  \
  \ ibuf ( -- ca )
  \
  \ Return the address _ca_ of the 100-byte insert buffer used
  \ by the `gforth-editor`.
  \
  \ See: `rbuf`, `fbuf`.
  \
  \ }doc

create fbuf $100 allot
  \ XXX TODO -- rename
  \ XXX TODO -- use the `stringer` instead?

  \ doc{
  \
  \ ibuf ( -- ca )
  \
  \ Return the address _ca_ of the 100-byte search buffer used
  \ by the `gforth-editor`..
  \
  \ See: `rbuf`, `ibuf`.
  \
  \ }doc

: h ( -- )
  r# @ c/l /mod swap >r scr @ line>string
  2dup drop r@ cr type
       r> /string 2dup drop len @ 1 inverse type 0 inverse
                       len @ /string type ;

  \ doc{
  \
  \ h ( -- )
  \
  \ A command of `gforth-editor`:
  \ Type the line of the marked area, highlighting it.
  \
  \ }doc

: g ( u -- ) page list h ;

  \ doc{a
  \
  \  g ( u -- )
  \
  \ A command of `gforth-editor`:
  \ Go to screen _u_.
  \
  \ }doc

: l ( -- ) scr @ g ;

  \ doc{
  \
  \  l ( -- )
  \
  \ A command of `gforth-editor`:
  \ Go to current screen.
  \
  \ }doc

: m ( -- ) scr @ r# @ mark 2! ;

  \ doc{
  \
  \ m ( -- )
  \
  \ A command of `gforth-editor`:
  \ Mark current position.
  \
  \ }doc

: a ( -- ) mark 2@ m r# ! g ;

  \ doc{
  \
  \ a ( -- )
  \
  \ A command of `gforth-editor`:
  \ Go to marked position, marking the current position first.
  \
  \ }doc

: c ( n -- ) r# +! 1 len ! l ; -->

  \ XXX TODO -- word to move cursor by _n_ lines, and putting
  \ it at the start of a line

  \ doc{
  \
  \  c ( n -- )
  \
  \ A command of `gforth-editor`:
  \ Move cursor by _n_ chars.
  \
  \ }doc

( gforth-editor )

: 'rest ( -- ca len ) scr @ block b/buf r# @ /string ;

  \ XXX TODO -- rename

  \ doc{
  \
  \  'rest ( -- ca len )
  \
  \ Part of the `gforth-editor`:
  \ Return the rest of the current screen, from the
  \ current position.
  \
  \ }doc


: 'line ( -- ca len ) 'rest 1- c/l 1- and 1+ ;

  \ XXX TODO -- rename

  \ doc{
  \
  \  'line ( -- ca len )
  \
  \ Part of the `gforth-editor`:
  \ Return the rest of the current line, from the
  \ current position.
  \
  \ }doc

: 'par ( buf "ccc<eol>" -- ca len ) >r 0 parse dup
  0= if 2drop r> count else 2dup  r> place then ;

  \ XXX TODO -- rename to `buffer-parse`

  \ doc{
  \
  \  'par ( buf "ccc<eol>" -- ca len )
  \
  \ Part of the `gforth-editor`:
  \ Parse _ccc_. If the result string is empty,
  \ discard it and return the counted string at _buf_;
  \ else return the parsed string and also store it
  \ at _buf_ as a counted string.
  \
  \ }doc

: t ( u "ccc<eol>" -- ) c/l * r# ! c/l len !
  0 parse tuck 'line insert if update then l ;

  \ XXX TODO -- make `len` the length of the inserted text

  \ doc{
  \
  \  t ( u "ccc<eol>" -- )
  \
  \ A command of `gforth-editor`:
  \ Go to line _u_ and insert _ccc_.
  \
  \ }doc

: i ( "ccc<eol>" -- ) ibuf 'par 'line insert update l ;

  \ doc{
  \
  \ i ( "ccc<eol>" -- )
  \
  \ A command of `gforth-editor`:
  \ Insert _ccc_ or, if it's empty, the contents of the insert
  \ buffer.
  \
  \ See: `ibuf`, `<<src-lib-prog-editor-gforth-fs, l>>`.
  \
  \ }doc

: d ( -- ) 'line 2dup rbuf place len @ delete update l ;

  \ doc{
  \
  \ d ( -- )
  \
  \ A command of `gforth-editor`:
  \ Delete marked area.
  \
  \ }doc

: r ( "ccc<eol>" -- ) d i ;

  \ doc{
  \
  \  r ( "ccc<eol>" -- )
  \
  \ A command of `gforth-editor`:
  \ Replace marked area.
  \
  \ }doc

: y ( -- ) rbuf count 'line insert update l ;

  \ doc{
  \
  \  y ( -- )
  \
  \ A command of `gforth-editor`:
  \ Yank deleted string.
  \
  \ }doc

: f ( "ccc<eol>" | -- )
  'rest len @ c/l mod /string fbuf 'par dup len ! search
  0= throw nip b/buf swap - r# ! l ;

  \ XXX TODO -- Confirm what happens if _ccc_ is empty.

  \ doc{
  \
  \  f ( "ccc<eol>" | -- )
  \
  \ A command of `gforth-editor`:
  \ Parse _ccc_, search it and mark it.
  \
  \ }doc

: il ( -- )
  pad c/l 'rest insert 'rest drop c/l blank update l ; -->

  \ doc{
  \
  \ il ( -- )
  \
  \ A command of `gforth-editor`:
  \ Insert a line at the cursor position.
  \
  \ }doc

( gforth-editor )

: dl ( -- ) 'rest c/l delete update l ;

  \ doc{
  \
  \ dl ( -- )
  \
  \ A command of `gforth-editor`:
  \ Delete a line at the cursor position.
  \
  \ }doc

: n ( -- ) scr @ 1+ top g ;

  \ doc{
  \
  \  n ( -- )
  \
  \ A command of `gforth-editor`:
  \ Go to next screen.
  \
  \ }doc

: p ( -- ) scr @ 1- top g ;

  \ doc{
  \
  \  p ( -- )
  \
  \ A command of `gforth-editor`:
  \ Go to previous screen.
  \
  \ }doc

: s ( u "ccc<eol>" | u -- )
  >r begin ['] f catch
     while scr @ r@ =  if rdrop exit then
           scr @ r@ u< if n else p   then repeat
  r> ;

  \ XXX TODO -- simplify and improve: search from the current
  \ block to the last one, without listing them; use
  \ `break-key?` to stop.

  \ doc{
  \
  \ s ( u "ccc<eol>" | u -- )
  \
  \ A command of `gforth-editor`:
  \ Search for _ccc_ until screen _u_. If _ccc_ is empty, use
  \ the string of the previous search.
  \
  \ }doc

forth definitions

  \ ===========================================================
  \ Change log

  \ 2016-11-19: Start. Adapt layout and requirements. Add
  \ comments. Remove `hi`.
  \
  \ 2016-11-20: Rename `l` to `g`; rename `v` to `l`, after the
  \ classic fig-Forth editor. Rename `b` to `p`.  Rename `bx`
  \ to `px`. Add comments. Try the original code with Gforth.
  \
  \ 2016-11-21: Improve, try and document. Adapt quick index
  \ and highlighting of marked area. Move quick index to
  \ <tool.list.blocks.fsb>.
  \
  \ 2016-11-22: Move `r#` to <editor.common.fsb>` and get `top`
  \ from it.
  \
  \ 2016-11-26: Need `catch`, which has been moved to the
  \ library.
  \
  \ 2017-03-12: Update mentions to the `stringer`.
  \
  \ 2018-02-27: Update header and source layout.
  \
  \ 2020-05-03: Rename filename to <prog.editor.gforth.fs>. Add
  \ `gforth-editor`.
  \
  \ 2020-05-04: Move `insert`, `delete`, `replace` to
  \ <strings.MISC.fs>.
  \
  \ 2020-05-05: Document `gforth-editor`.
  \
  \ 2020-05-11: Improve documentation.

  \ vim: filetype=soloforth
