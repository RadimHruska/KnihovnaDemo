import { NextResponse } from 'next/server'

export async function GET() {
  try {
    const response = await fetch('http://localhost:5000/api/lends')
    if (!response.ok) {
      throw new Error('Failed to fetch lends')
    }
    const data = await response.json()
    return NextResponse.json(data)
  } catch (error) {
    console.error('Error fetching lends:', error)
    return NextResponse.json(
      { message: 'Chyba při načítání výpůjček' },
      { status: 500 }
    )
  }
}

export async function POST(request: Request) {
  try {
    const body = await request.json()
    const response = await fetch('http://localhost:5000/api/lends', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(body),
    })

    if (!response.ok) {
      const error = await response.json()
      return NextResponse.json(
        { message: error.message || 'Chyba při vytváření výpůjčky' },
        { status: 400 }
      )
    }

    const data = await response.json()
    return NextResponse.json(data)
  } catch (error) {
    console.error('Error creating lend:', error)
    return NextResponse.json(
      { message: 'Chyba při vytváření výpůjčky' },
      { status: 500 }
    )
  }
} 