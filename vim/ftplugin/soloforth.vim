" ~/.vim/ftplugin/soloforth.vim

" Vim filetype plugin for Solo Forth

" This file is part of Solo Forth
" http://programandala.net/en.program.solo_forth.html

" --------------------------------------------------------------
" Author

" Marcos Cruz (programandala.net), 2015, 2017.

" --------------------------------------------------------------
" History

" 2015-09-20: First version.
"
" 2017-02-28: Restore the disambling check. Add `runtime
" fsb.vim`. Add `colorcolumn` and `cursorline`.

" --------------------------------------------------------------
" Disabling

" Only do this when not done yet for this buffer

" This also needs to be used to avoid that the same plugin is
" executed twice for the same buffer (happens when using an
" ":edit" command without arguments).

if exists("b:did_ftplugin")
  finish
endif
let b:did_ftplugin = 1

" --------------------------------------------------------------
" Comments

" XXX TODO -- Finish.

"setlocal comments=b:\\,sb2:(,m:\s\s,e2:)
"setlocal comments=b:\\,sb2:(,ex:)
setlocal comments=b:\\

" Used by the Vim-Commentary plugin:
setlocal commentstring=\\\ %s

" --------------------------------------------------------------
" Format options

setlocal textwidth=63
set colorcolumn=64
set cursorline

"setlocal formatoptions-=t
"setlocal formatoptions+=cqor
"setlocal formatoptions-=r
setlocal formatoptions=cqorj

" Note: The "j" formatoptions flag removes a comment leader
" when joining lines.  See ":help fo-table".

setlocal tabstop=2
setlocal softtabstop=0
setlocal shiftwidth=2
setlocal expandtab
setlocal ignorecase
setlocal smartcase
setlocal smartindent

" --------------------------------------------------------------
" Key mapping

" Use the maps defined by fsb
" (http://programandala.net/en.program.fsb.html), which must be
" instaled:

runtime fsb.vim
