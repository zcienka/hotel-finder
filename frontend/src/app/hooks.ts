import {TypedUseSelectorHook, useDispatch, useSelector} from 'react-redux'
import {AppDispatch, RootState} from "./store"
import {Dispatch} from "react";
import {AnyAction} from "@reduxjs/toolkit";

export const useAppDispatch: () => AppDispatch = useDispatch
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector
