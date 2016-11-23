" ~/.vim/ftdetect/soloforth.vim

" Vim filetype detect for the Forth source formats of Solo Forth

" By Marcos Cruz (programandala.net)

" This file is part of Solo Forth
" http://programandala.net/en.program.solo_forth.html

" 2016-11-20: First version.

autocmd BufNewFile,BufRead *.fsb setlocal filetype=soloforth
autocmd BufNewFile,BufRead *.fsb runtime fsb.vim

autocmd BufNewFile,BufRead *.fsa setlocal filetype=soloforth
