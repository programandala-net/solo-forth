  \ prog.editor.blocked.fs
  \
  \ This file is part of Solo Forth
  \ http://programandala.net/en.program.solo_forth.html

  \ XXX UNDER DEVELOPMENT

  \ Last modified: 202005030223
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

  \ ===========================================================
  \ Usage

  \ m   mark current position
  \ a   go to marked position
  \ c   move cursor by given number of chars
  \ t   go to a given line and inserts
  \ i   insert
  \ d   delete marked area
  \ r   replace marked area
  \ f   search and mark
  \ il  insert a line
  \ dl  delete a line
  \ n   go to next screen
  \ p   go to previous screen
  \ g   go to a given screen
  \ l   list current screen
  \ s   search until a given screen
  \ y   yank deleted string

( gforth-editor )

only forth definitions

need inverse need list need update
need list-lines need vocabulary need catch need editor
need insert need replace need delete

vocabulary gforth-editor ' gforth-editor is editor
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
  \ Replace buffer.
  \ XXX TODO -- rename
  \ XXX TODO -- use the `stringer` instead?

create ibuf $100 allot
  \ Insert buffer.
  \ XXX TODO -- rename
  \ XXX TODO -- use the `stringer` instead?

create fbuf $100 allot
  \ Search buffer.
  \ XXX TODO -- rename
  \ XXX TODO -- use the `stringer` instead?

: h ( -- )
  r# @ c/l /mod swap >r scr @ line>string
  2dup drop r@ cr type
       r> /string 2dup drop len @ 1 inverse type 0 inverse
                       len @ /string type ;
  \ Type the line of the marked area, highlighting it.

: g ( u -- ) page list h ;
  \ Go to screen _u_.

: l ( -- ) scr @ g ;
  \ Go to current screen.

: m ( -- ) scr @ r# @ mark 2! ;
  \ Mark current position.

: a ( -- ) mark 2@ m r# ! g ;
  \ Go to marked position, marking the current position first.

: c ( n -- ) r# +! 1 len ! l ;  -->
  \ Move cursor by _n_ chars.

  \ XXX TODO -- word to move cursor by _n_ lines, and putting
  \ it at the start of a line

( gforth-editor )

: 'rest ( -- ca len ) scr @ block b/buf r# @ /string ;
  \ Return the rest of the current screen, from the
  \ current position.
  \ XXX TODO -- rename

: 'line ( -- ca len ) 'rest 1- c/l 1- and 1+ ;
  \ Return the rest of the current line, from the
  \ current position.
  \ XXX TODO -- rename

: 'par ( buf "ccc<eol>" -- ca len ) >r 0 parse dup
  0= if 2drop r> count else 2dup  r> place then ;
  \ Parse _ccc_. If the result string is empty,
  \ discard it and return the counted string at _buf_;
  \ else return the parsed string and also store it
  \ at _buf_ as a counted string.
  \ XXX TODO -- rename to `buffer-parse`

: t ( u "ccc<eol>" -- ) c/l * r# ! c/l len !
  0 parse tuck 'line insert if update then l ;
  \ Go to line _u_ and insert _ccc_.
  \ XXX TODO -- make `len` the length of the inserted text

: i ( "ccc<eol>" -- ) ibuf 'par 'line insert update l ;
  \ Insert _ccc_ or, if it's empty, the contents of the insert
  \ buffer.

: d ( -- ) 'line 2dup rbuf place len @ delete update l ;
  \ Delete marked area.

: r ( "ccc<eol>" -- ) d i ;
  \ Replace marked area.

: y ( -- ) rbuf count 'line insert update l ;
  \ Yank deleted string.

: f ( "ccc<eol>" | -- )
  'rest len @ c/l mod /string fbuf 'par dup len ! search
  0= throw nip b/buf swap - r# ! l ;
  \ Parse _ccc_, search it and mark it. If _ccc_ is empty, use
  \ the

: il ( -- )
  pad c/l 'rest insert 'rest drop c/l blank update l ;  -->
  \ Insert a line at the cursor position.

( gforth-editor )

: dl ( -- ) 'rest c/l delete update l ;
  \ Delete a line at the cursor position.

: n ( -- ) scr @ 1+ top g ;
  \ Go to next screen.

: p ( -- ) scr @ 1- top g ;
  \ Go to previous screen.

: s ( u "ccc<eol>" | u -- ) >r
  begin ['] f catch
  while scr @ r@ = if rdrop exit then
        scr @ r@ u< if n else p then
  repeat r> ;
  \ Search for _ccc_ until screen _u_. If _ccc_ is empty, use
  \ the string of the previous search.
  \
  \ XXX TODO -- simplify and improve: search from the current
  \ block to the last one, without listing them; use
  \ `break-key?` to stop.

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

  \ vim: filetype=soloforth
