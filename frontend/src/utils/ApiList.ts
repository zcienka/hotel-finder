export type ApiList<TypeGeneric> = {
    count: number
    next: string | null
    previous: string | null
    results: TypeGeneric[]
}