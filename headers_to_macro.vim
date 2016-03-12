" headers_to_macro.vim
" This file is part of the development of Solo Forth.
" This file converts the headers to macros.
"
" 2015-06-18


if 1

" code headers
%s@^\([a-z0-9_]\{-}_\)nfa:\n  db 0x[0-9A-F]\{2}+0x80,\('.*'\),\('.\{-}'\)+0x80\n\1lfa:\n  dw 0x0000\n\1:\n  dw \1pfa\n\1pfa:@  _code_header \1,\2,\3\r@
%s@^\([a-z0-9_]\{-}_\)nfa:\n  db 0x[0-9A-F]\{2}+0x80,\('.*'\),\('.\{-}'\)+0x80\n\1lfa:\n  dw [a-z0-9_]\+\n\1:\n  dw \1pfa\n\1pfa:@  _code_header \1,\2,\3\r@
%s@^\([a-z0-9_]\{-}_\)nfa:\n  db 0x[0-9A-F]\{2}+0x80,\('.*'\),\('.\{-}'\)+0x80\n\1lfa:\n  dw [a-z0-9_]\+\n\1:\n  dw \(\S\{-}_\)pfa@  _code_alias_header \1,\2,\3,,\4\r@

" non-code headers
%s@^\([a-z0-9_]\{-}_\)nfa:\n  db 0x[0-9A-F]\{2}+0x80,\('.*'\),\('.\{-}'\)+0x80\n\1lfa:\n  dw [a-z0-9_]\+\n\1:\n  dw do_\([a-z]\+\)\n\1pfa:@  _\4_header \1,\2,\3\r@
%s@^\([a-z0-9_]\{-}_\)nfa:\n  db 0x[0-9A-F]\{2}+0x80,\('.*'\),\(".\{-}"\)+0x80\n\1lfa:\n  dw [a-z0-9_]\+\n\1:\n  dw do_\([a-z]\+\)\n\1pfa:@  _\4_header \1,\2,\3\r@
%s@^\([a-z0-9_]\{-}_\)nfa:\n  db 0x[0-9A-F]\{2}+0x80+precedence_bit_mask,\('.*'\),\('.\{-}'\)+0x80\n\1lfa:\n  dw [a-z0-9_]\+\n\1:\n  dw do_\([a-z]\+\)\n\1pfa:@  _\4_header \1,\2,\3,immediate\r@

" bracket-tick
%s@^\([a-z0-9_]\{-}_\)nfa:\n  db 0x[0-9A-F]\{2}+0x80+precedence_bit_mask,\(....\),\(...\)+0x80\n\1lfa:\n  dw [a-z0-9_]\+\n\1:\n  dw do_\([a-z]\+\)\n\1pfa:@  _\4_header \1,\2,\3,immediate\r@
" null word
%s@^\([a-z0-9_]\{-}_\)nfa:\n  db 0x[0-9A-F]\{2}+0x80+precedence_bit_mask,'',0x80\n\1lfa:\n  dw [a-z0-9_]\+\n\1:\n  dw do_\([a-z]\+\)\n\1pfa:@  _colon_header \1,'',0,immediate\r@

endif
