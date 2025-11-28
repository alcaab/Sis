namespace Desyco.Dms.Domain.Payments;

public enum PaymentConceptType
{
// --- 1. Mandatory / Base Charges ---
    EnrollmentFee,          // Annual registration or matriculation fee
    ReEnrollmentFee,        // Fee for returning students to secure their spot
    MonthlyTuition,         // Recurring monthly payment for education
    TechnologyFee,          // Charge for digital platforms, LMS, or computer labs
    ParentsAssociationFee,  // PTA (Parent-Teacher Association) membership fee

    // --- 2. Optional Services ---
    TransportationService,  // School bus or shuttle service
    DiningService,          // Cafeteria, lunch plans, or meal tickets
    ExtendedCare,           // After-school care / Daycare services

    // --- 3. Physical Goods ---
    Books,                  // Textbooks and workbooks
    Uniforms,               // Sports or gala uniforms sold by the school
    SchoolSupplies,         // Stationery, art supplies, or material lists
    IdCardReplacement,      // Fee for replacing a lost student ID card

    // --- 4. Academic & Administrative ---
    CertificateIssuance,    // Issuing transcripts, report cards, or official letters
    GraduationFee,          // Costs related to the graduation ceremony (gown, diploma)
    ExamRetakeFee,          // Fee for remedial exams or validation tests
    AdmissionExamFee,       // Entrance exam fee for new applicants

    // --- 5. Activities ---
    ExtracurricularActivity,// Sports clubs, music lessons, robotics, etc.
    FieldTrip,              // Excursions, museums, or external visits
    SchoolEvents,           // Tickets for plays, family days, or festivals

    // --- 6. Penalties & Adjustments ---
    LatePaymentPenalty,     // Surcharge for paying after the due date
    DamageRepair,           // Charge for damaging school property
    ReturnedCheckFee,       // Penalty for insufficient funds / bounced checks

    // --- 7. Miscellaneous ---
    Donation,               // Voluntary financial contributions
    Other                   // Uncategorized charges
}
